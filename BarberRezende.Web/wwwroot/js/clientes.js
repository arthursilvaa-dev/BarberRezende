// clientes.js
// Se você tinha confirm() aqui, REMOVA.
// A confirmação oficial é a tela /Clientes/Delete/{id}.

console.log("clientes.js carregado"); // Apenas para verificar se o arquivo está sendo carregado corretamente.


// Filtro simples no front: filtra linhas por nome/email/telefone
(function () {
    const input = document.getElementById("clientesSearch");
    const table = document.getElementById("clientesTable");

    if (!input || !table) return;

    input.addEventListener("input", function () {
        const q = (input.value || "").toLowerCase().trim();
        const rows = table.querySelectorAll("tbody tr");

        rows.forEach(r => {
            const nome = (r.querySelector(".col-nome")?.innerText || "").toLowerCase();
            const tel = (r.querySelector(".col-tel")?.innerText || "").toLowerCase();
            const email = (r.querySelector(".col-email")?.innerText || "").toLowerCase();

            const ok = nome.includes(q) || tel.includes(q) || email.includes(q);
            r.style.display = ok ? "" : "none";
        });
    });
})();
