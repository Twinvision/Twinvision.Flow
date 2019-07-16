@ModelType IndexViewModel
@Code
    ViewBag.Title = "Manage"
End Code

<h2>@ViewBag.Title.</h2>

<p class="text-success">@ViewBag.StatusMessage</p>
<div>
    <h4>Change your account settings</h4>
    <hr />
    <dl class="dl-horizontal">
        <dt>Password:</dt>
        <dd>
            [
            @If Model.HasPassword Then
                @Html.ActionLink("Change your password", "ChangePassword")
            Else
                @Html.ActionLink("Create", "SetPassword")
            End If
            ]
        </dd>
        <dt>External Logins:</dt>
        <dd>
            @Model.Logins.Count [
            @Html.ActionLink("Manage", "ManageLogins") ]
        </dd>
        @*
            Phone Numbers can used as a second factor of verification in a two-factor authentication system.
             
             See <a href="http://go.microsoft.com/fwlink/?LinkId=313242">this article</a>
                for details on setting up this ASP.NET application to support two-factor authentication using SMS.
             
             Uncomment the following block after you have set up two-factor authentication
        *@
        @* 
            <dt>Phone Number:</dt>
            <dd>
                @(If(Model.PhoneNumber, "None")) [
                @If (Model.PhoneNumber <> Nothing) Then
                    @Html.ActionLink("Change", "AddPhoneNumber")
                    @: &nbsp;|&nbsp;
                    @Html.ActionLink("Remove", "RemovePhoneNumber")
                Else
                    @Html.ActionLink("Add", "AddPhoneNumber")
                End If
                ]
            </dd>
        *@
        <dt>Two-Factor Authentication:</dt>
        <dd>
            <p>
                There are no two-factor authentication providers configured. See <a href="http://go.microsoft.com/fwlink/?LinkId=313242">this article</a>
                for details on setting up this ASP.NET application to support two-factor authentication.
            </p>
            @*
		@If Model.TwoFactor Then
                    @<form method="post" action="/Manage/DisableTwoFactorAuthentication">
                        Enabled
                        <input type="submit" value="Disable" class="btn btn-link" />
                    </form>
                Else
                    @<form method="post" action="/Manage/EnableTwoFactorAuthentication">
                        Disabled
                        <input type="submit" value="Enable" class="btn btn-link" />
                    </form>
                End If
	     *@
        </dd>
    </dl>
</div>
