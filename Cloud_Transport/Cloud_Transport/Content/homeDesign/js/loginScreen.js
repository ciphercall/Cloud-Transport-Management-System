$(document).ready(function() {
    $("#login").click(function() {
        $("#loginScreen").fadeIn()
    }),$("#closeForm").click(function() {
        $("#loginScreen").fadeOut()
    }),$("#btnForgotPass").click(function() {
        $("#loginBody").hide(),$("#passwordResetBody").show()
    }),$("#btnLoginBody").click(function() {
        $("#loginBody").show(),$("#passwordResetBody").hide()
    })
});