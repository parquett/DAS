// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Account.cs" company="SecurityCRM">
//   Copyright 2020
// </copyright>
// <summary>
//   Defines the Account type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SecurityCRMweblib.Controllers
{
    using SecurityCRMLib.BusinessObjects;
    using System.Collections.Generic;
    using Lib.Tools.BO;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.ViewEngines;
    using System.Threading.Tasks;
    using SecurityCRMLib.BusinessObjects;

    /// <summary>
    /// The account.
    /// </summary>
    public class AccountWebController : Weblib.Controllers.AccountBaseController
    {
        public AccountWebController(ICompositeViewEngine viewEngine) : base(viewEngine)
        {
        }

        /// <summary>
        /// The login.
        /// </summary>
        /// <param name="returnUrl">
        /// The return url.
        /// </param>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        public new ActionResult Login(string returnUrl)
        {
            var SortParameters =  new List<SortParameter>();
            SortParameters.Add(new SortParameter(){Field="Date",Direction="desc"});
            this.ViewData["News"] = (new News()).Populate(SortParameters);
            this.ViewData["Information"] = (new UsefullInfo()).Populate();
            return base.Login(returnUrl);
        }

        /// <summary>
        /// The login.
        /// </summary>
        /// <param name="user">
        /// The user.
        /// </param>
        /// <param name="returnUrl">
        /// The return url.
        /// </param>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public Task<ActionResult> Login(User user, string returnUrl)
        {
            return base.Login(user, returnUrl);
        }

        /// <summary>
        /// The login.
        /// </summary>
        /// <param name="returnUrl">
        /// The return url.
        /// </param>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        public ActionResult CPLogin(string returnUrl)
        {
            return base.CPLogin(returnUrl);
        }

        /// <summary>
        /// The login.
        /// </summary>
        /// <param name="user">
        /// The user.
        /// </param>
        /// <param name="returnUrl">
        /// The return url.
        /// </param>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CPLogin(User user, string returnUrl)
        {
            return base.CPLogin(returnUrl);
        }

        /// <summary>
        /// The login.
        /// </summary>
        /// <param name="returnUrl">
        /// The return url.
        /// </param>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        public ActionResult SMILogin(string returnUrl)
        {
            return base.SMILogin(returnUrl);
        }

        /// <summary>
        /// The login.
        /// </summary>
        /// <param name="user">
        /// The user.
        /// </param>
        /// <param name="returnUrl">
        /// The return url.
        /// </param>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SMILogin(User user, string returnUrl)
        {
            return base.SMILogin( returnUrl);
        }
    }
    
}
