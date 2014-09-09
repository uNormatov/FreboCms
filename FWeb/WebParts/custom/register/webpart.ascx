<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="webpart.ascx.cs" Inherits="FWeb.WebParts.custom.register.webpart" %>
<script src="/themes/kitobim/validator/js/languages/jquery.validationEngine-en.js"
    type="text/javascript"></script>
<script src="/themes/kitobim/validator/js/jquery.validationEngine.js" type="text/javascript"></script>
<link href="/themes/kitobim/validator/css/validationEngine.jquery.css" rel="stylesheet"
    type="text/css" />
<script type="text/javascript">
    $(document).ready(function () {
        $("#formLogin").validationEngine();
        $("#formRegister").validationEngine();
    });
</script>
<div style="margin-left: 30px; margin-right: 10px;">
    <div class="login-kitobim left">
        <form method="POST" action="/login" id="formLogin">
        <input type="hidden" name="action" id="action" value="0" />
        <h2>
            Kirish</h2>
        <label>
            <span>Login (el.pochta)</span>
            <input type="text" id="login" name="login" class="txt validate[required,custom[email]]" />
        </label>
        <label>
            <span>Parol</span>
            <input type="password" id="password" name="password" class="txt validate[required,minSize[6]] text-input" />
        </label>
        <a href="#" class="right">Qayta tiklash?</a>
        <label>
            <input type="checkbox" checked="checked" class="chk" runat="server" id="chbxRemember" />
            Eslab qolish.
        </label>
        <input type="submit" id="submit" value="Login" class="btn" />
        </form>
    </div>
    <div class="reg-kitobim right">
        <form method="POST" action="/login" id="formRegister">
        <input type="hidden" name="action" id="action" value="1" />
        <h2>
            Ro'yxatdan o'tish</h2>
        <label>
            <span>Elektron pochta</span>
            <input type="text" id="login" name="login" class="txt validate[required,custom[email]]" />
        </label>
        <label>
            <span>Parol</span>
            <input type="password" id="password" name="password" class="txt validate[required,minSize[6]] text-input" />
        </label>
        <label>
            <input type="checkbox" checked="checked" class="chk" runat="server" id="chbxShart" />
            <a target="_blank" href="/shartlar">Shartlarga</a> roziman
        </label>
        <input type="submit" id="submit1" value="Login" class="btn" />
        </form>
    </div>
    <div class="login-socialnetwork">
        <h2>
            Ijtimoiy tarmoqlar orqali</h2>
        <a href="/" class="acc-vkontkte acc-login">Войти через Вконтакте</a> <a href="/"
            class="acc-facebook acc-login">Войти через Facebook</a> <a href="/" class="acc-odnoklassniki acc-login">
                Войти через Одноклассники</a> <a href="/" class="acc-twitter acc-login">Войти через
                    Twitter</a>
    </div>
</div>
