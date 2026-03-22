// wwwroot/js/telefone-mask.js
// Máscara BR para telefone:
// - Digita: 11999998888
// - Vira:  (11) 99999-8888
// - Impede ultrapassar 11 dígitos reais
// - Atualiza enquanto digita

(function () {
  function onlyDigits(value) {
    return (value || "").replace(/\D/g, "");
  }

  function formatPhoneBR(digits) {
    // máximo 11 dígitos (2 DDD + 9 número)
    digits = digits.substring(0, 11);

    if (digits.length <= 2) return digits.length ? `(${digits}` : "";
    if (digits.length <= 6) return `(${digits.substring(0, 2)}) ${digits.substring(2)}`;

    // 11 dígitos -> (11) 99999-9999
    // 10 dígitos -> (11) 9999-9999
    const ddd = digits.substring(0, 2);
    const rest = digits.substring(2);

    if (rest.length <= 8) {
      // 10 dígitos total
      return `(${ddd}) ${rest.substring(0, 4)}-${rest.substring(4)}`;
    }

    // 11 dígitos total
    return `(${ddd}) ${rest.substring(0, 5)}-${rest.substring(5)}`;
  }

  function applyMask(input) {
    const digits = onlyDigits(input.value);
    input.value = formatPhoneBR(digits);
  }

  function setCursorEnd(input) {
    // pra não brigar com cursor: mantém no final (simples e funcional)
    const len = input.value.length;
    input.setSelectionRange(len, len);
  }

  function bindPhoneMask(selector) {
    const inputs = document.querySelectorAll(selector);
    inputs.forEach((input) => {
      // Ajuda o mobile a abrir teclado numérico
      input.setAttribute("inputmode", "numeric");
      // O texto formatado tem no máximo 15 caracteres: "(11) 99999-9999"
      input.setAttribute("maxlength", "15");

      // Aplica se já veio preenchido (edit)
      applyMask(input);

      input.addEventListener("input", function () {
        applyMask(input);
        setCursorEnd(input);
      });

      input.addEventListener("blur", function () {
        applyMask(input);
      });
    });
  }

  // Expõe global pra você usar em páginas específicas, se quiser:
  window.TelefoneMask = { bindPhoneMask };
})();
