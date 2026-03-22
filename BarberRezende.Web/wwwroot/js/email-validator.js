// wwwroot/js/email-validator.js
// Validação simples de e-mail (visual apenas)

(function () {

  function isValidEmail(email) {
    if (!email) return true; // não obriga, só valida se tiver algo

    const regex = /^[^\s@]+@[^\s@]+\.[^\s@]{2,}$/;
    return regex.test(email.trim());
  }

  function bindEmailValidation(selector) {
    const inputs = document.querySelectorAll(selector);

    inputs.forEach((input) => {

      input.setAttribute("autocomplete", "email");

      input.addEventListener("input", function () {

        if (!input.value) {
          input.classList.remove("is-invalid");
          return;
        }

        if (!isValidEmail(input.value)) {
          input.classList.add("is-invalid");
        } else {
          input.classList.remove("is-invalid");
        }

      });

    });
  }

  window.EmailValidator = { bindEmailValidation };

})();
