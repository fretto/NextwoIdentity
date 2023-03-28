using Azure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.VisualBasic;
using Microsoft.EntityFrameworkCore;
using NextwoIdentity.Data;
using NextwoIdentity.Models;
using NextwoIdentity.Models.ViewModels;
using System.Data;

namespace NextwoIdentity.Controllers
{
    [Authorize]
    public class AccountController : Controller //inheritance from the built in controller class
    {
        #region configuration
        private UserManager<IdentityUser> userManager;
        private SignInManager<IdentityUser> signInManager;
        private RoleManager<IdentityRole> roleManager;
        private readonly ILogger<HomeController> _logger;
        private NextwoDbContext db;
        //create a constructor to inject the services //constructor injection //dependency injection
        public AccountController(ILogger<HomeController> logger, NextwoDbContext _db,UserManager<IdentityUser> _userManager, SignInManager<IdentityUser> _signInManager, RoleManager<IdentityRole> _roleManager)
        {
            userManager = _userManager;
            signInManager = _signInManager;
            roleManager= _roleManager;
            _logger = logger;
            db = _db;
        }

        #endregion

        #region User
        [AllowAnonymous]

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]

        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {

                IdentityUser user = new IdentityUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    PhoneNumber = model.Phone

                };
                var result = await userManager.CreateAsync(user, model.Password!);
                if (result.Succeeded)
                {

                    return RedirectToAction("Login", "Account");//login in the account controller
                }
                foreach (var err in result.Errors)
                {

                    ModelState.AddModelError(err.Code, err.Description);
                }
                return View(model);

            }

            return View(model);

        }
        [AllowAnonymous]

        public IActionResult Login()
        {

            return View();

        }

        [HttpPost]
        [AllowAnonymous]

        public async Task <IActionResult> Login (LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await signInManager.PasswordSignInAsync(model.Email!, model.Password!, false, false);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");//index view in the home controller
                }
                ModelState.AddModelError("", "Invalid user or password");
                return View(model);
            }
            return View(model);
        }
        [AllowAnonymous] //This attribute allows anonymous users to access certain Controllers/Actions

        public async Task <IActionResult> logout()
        {


            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");

        }

        #endregion

        #region Roles

        [Authorize(Roles = "admin")]//accessed only by the authenticated and authorized users as admin

        public IActionResult CreateRole()
        {

            return View();
        }
        [HttpPost]
        [Authorize(Roles = "admin")]

        public async Task<IActionResult> CreateRole(CreateRoleViewModel model)
        {
            if (ModelState.IsValid) {

                IdentityRole role = new IdentityRole
                {

                    Name = model.RoleName

                };
        
            var result = await roleManager.CreateAsync(role);
            
            if(result.Succeeded) {

                return RedirectToAction("RolesList");
            }
            
            foreach(var err in result.Errors) {

                ModelState.AddModelError(err.Code, err.Description);
            }
            return View(model);
            }

                    return View(model);

    }
        [Authorize(Roles = "admin")]

        public IActionResult RolesList()
        {
            return View(roleManager.Roles);
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
       // 21/3/2023 Tuesday///////////


    [HttpGet]
    public async Task <IActionResult>EditRole(string id)
        {
            if (id == null)
            {
                return RedirectToAction("RolesList");
            }
            var role = await roleManager.FindByIdAsync(id);
            if (role == null)
            {

                return RedirectToAction("RolesList");
            }
            EditRoleViewModel model = new EditRoleViewModel { RoleId = role.Id, RoleName = role.Name };

            foreach(var user in userManager.Users)
            {

                if(await userManager.IsInRoleAsync(user,role.Name!)) {

                    model.Users!.Add(user.UserName!);
                }
            }

            return View(model);
        }

        [HttpPost]


        public async Task<IActionResult> EditRole(EditRoleViewModel model)
        {

            if (ModelState.IsValid)
            {

                var role = await roleManager.FindByIdAsync(model.RoleId!);
                if (role == null)
                {

                    return RedirectToAction(nameof(ErrorPage));
                }
                role.Name = model.RoleName;
                var result = await roleManager.UpdateAsync(role);
                if (result.Succeeded)
                {


                    return RedirectToAction(nameof(RolesList));
                }
                foreach (var err in result.Errors)
                {


                    ModelState.AddModelError(err.Code, err.Description);
                }

                return View(model);
            }
            return View(model);


        }

        public IActionResult ErrorPage()
        {

            return View();
        }

        public async Task <IActionResult> ModifyUserInRole(string id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(RolesList));

            }
            var role=await roleManager.FindByIdAsync(id);
            if (role == null)
            {

                return RedirectToAction("ErrorPage");

            }
            List<UserRoleViewModel> models = new List<UserRoleViewModel>();
            foreach(var user in userManager.Users)
            {
                UserRoleViewModel userRole = new UserRoleViewModel
                {
                    UserId = user.Id,
                    UserName = user.UserName

                };
                if(await userManager.IsInRoleAsync(user, role.Name!))
                {

                    userRole.IsSelected= true;
                }
                else
                {
                    userRole.IsSelected = false;

                }
                models.Add(userRole);


            }
            return View(models);

        }

        [HttpPost]
        public async Task<IActionResult> ModifyUserInRole(string id,List<UserRoleViewModel> models)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(RolesList));
            }
            var role = await roleManager.FindByIdAsync(id);
            if (role == null)
            {

                return RedirectToAction(nameof(ErrorPage));
            }

            for(int i=0; i<models.Count; i++)
            {
                IdentityResult result = new IdentityResult();
                var user = await userManager.FindByIdAsync(models[i].UserId!);
                if (models[i].IsSelected && ( !await userManager.IsInRoleAsync(user!, role.Name!)))
                {
                    result=await userManager.AddToRoleAsync(user!,role.Name!);


                }
                else if (!models[i].IsSelected && (await userManager.IsInRoleAsync(user!, role.Name!)))
                {

                    result = await userManager.RemoveFromRoleAsync(user!, role.Name!);


                }
                if (result.Succeeded) {

                    return RedirectToAction(nameof(RolesList));
                
                }
                return View(models);


            }
            return View(models);


        }

        #endregion

        #region products
        public IActionResult AllProducts()
        {


            return View(db.products.Include(x=>x.Category));

           
        }

        [HttpGet]
        public IActionResult CreateProduct()
        {

            ViewBag.categories = new SelectList(db.categories,"CategoryId" ,"CategoryName");

            return View();
        }

        [HttpPost]

        public IActionResult CreateProduct(Product product)
        {
            if (ModelState.IsValid)
            {
            db.products.Add(product);
            db.SaveChanges();
                return RedirectToAction("AllProducts");

            }
            
            
            return View(product);


        }

        [HttpGet]

        public IActionResult EditProduct(int? id)
        {

            ViewBag.categories = new SelectList(db.categories, "CategoryId", "CategoryName");

            if (id == null)
            {
                return RedirectToAction("AllProducts");
            }
            var pr = db.products.Find(id);
            if (pr == null)
            {
                return RedirectToAction("AllProducts");
            }
           
            return View(pr);

        }

        [HttpPost]

        public IActionResult EditProduct(Product product)
        {
            if (ModelState.IsValid)
            {
                db.products.Update(product);
                db.SaveChanges();
                return RedirectToAction("AllProducts");


            }
            return View(product);

        }

        [HttpGet]
        public IActionResult DeleteProduct(int? id) {

            if (id == null)
            {
                return RedirectToAction("AllProducts");
            }
            var pr = db.products.Find(id);
            if (pr == null)
            {
                return RedirectToAction("AllProducts");
            }

            return View(pr);



        }
        [HttpPost]
        public IActionResult DeleteProduct(Product product) {
            var pr = db.products.Find(product.ProductId);
            if (pr == null) {
                return RedirectToAction("AllProducts");
            }

            db.products.Remove(product);
            db.SaveChanges();
            return RedirectToAction("AllProducts");
        }
        [HttpGet]
        public IActionResult ProductDetails(int? id)

        {

            if (id == null)
            {
                return RedirectToAction("AllProducts");
            }
            var pr=db.products.Find(id);
            if(pr==null) {
                return RedirectToAction("AllProducts");
            }


            return View(pr);

        }

        #endregion 
    }
}
