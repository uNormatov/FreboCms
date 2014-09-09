<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="webpart.ascx.cs" Inherits="FWeb.WebParts.custom.edulogin.webpart" %>
<script src="/themes/eduportal/validator/js/languages/jquery.validationEngine-en.js"
    type="text/javascript"></script>
<script src="/themes/eduportal/validator/js/jquery.validationEngine.js" type="text/javascript"></script>
<link href="/themes/eduportal/validator/css/validationEngine.jquery.css" rel="stylesheet"
    type="text/css" />
<script type="text/javascript">
    $(document).ready(function () {
        $("#formLogin").validationEngine();
        $("#formRegister").validationEngine();
    });
</script>
<div class="login-container" style="margin-left: 10px; margin-right: 10px;">
    <div class="login-eduuz left">
        <form method="POST" action="/login" id="formLogin">
        <h2>
            <%= GetResource("lblKirish") %></h2>
        <label>
            <span>
                <%= GetResource("lblLogin") %></span>
            <input type="text" id="login" name="login" class="txt validate[required,custom[email]]" />
        </label>
        <label>
            <span>
                <%= GetResource("lblParol") %></span>
            <input type="password" id="password" name="password" class="txt validate[required,minSize[6]] text-input" />
        </label>
        <label>
            <input type="checkbox" class="chk" name="remember" />
            <%= GetResource("lblYoddaSaqlash") %>
        </label>
        <input type="submit" id="submit" value="Login" class="btn" />
        </form>
    </div>
</div>
