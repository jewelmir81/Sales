<h1>This application is a sample on Action based Authorization</h1?
<hr />
## Framework: .NET 5.0 ##

**<u>USERS</u>:**<br />
1. User ID: admin; Password: 123456<br />
2. User ID: Jewel; Password: 123456<br />
3. User ID: Mir; Password: 123456
## ##
<h3>Description</h3>
<p>The purpose of this application is to use the actions of the controllers as role and generate menu based on roles. I've shown it here, how to do this.</p>
<p>For that purpose I've created a custom authorize attribute named CustomAuthorizeAttribute that takes controller name and the action name as parametera and applied it on the action as attribute where I want to apply.</p>
<p>I've created a RolesForMenu static class to create role based menu. It has two static methods: GetMenus and GetMenu.</p>
<p>I've called GetMenus from Layout to generate role based menu. Then I've called GetMenu from controller's Index action to pass only authorized action link to the Index view.</p>
<h3><a href="http://jewel.features.site" target="_blank">SYED ZAHIDUL HASSAN</a></h3>
<address>
    77/C, Jonaki Road<br />
    Ahammed Nagar, Mirpur<br />
    Dhaka-1216, Dhaka, Bangladesh<br />
    <abbr title="Phone">P:</abbr>
    +88(018) 17 015 015
</address>
<br />
<address>
    <strong>LIVE:</strong>   <a href="mailto:jewelmir@live.com">jewelmir@live.com</a><br />
    <strong>GMAIL:</strong> <a href="mailto:jewelmir81@gmail.com">jewelmir81@gmail.com</a><br />
    <strong>Website:</strong> <a href="http://features.site/" target="_blank">features.site</a>
</address></p>
