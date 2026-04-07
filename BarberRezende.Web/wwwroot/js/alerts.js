/* =========================================
   HANDLE CENTRAL DE ALERTAS PREMIUM (SwAL2)
   ========================================= */

// Configuração base do SweetAlert2 para combinar com o Tema Dark & Gold
const swalDark = Swal.mixin({
    customClass: {
        confirmButton: 'btn btn-gold px-4 py-2 rounded-pill',
        cancelButton: 'btn btn-outline-danger px-4 py-2 rounded-pill'
    },
    buttonsStyling: false,
    background: '#161926', // var(--bg-card)
    color: '#ffffff', // var(--text-main)
    confirmButtonColor: '#D4AF37' // var(--accent-gold)
});

// Função universal para mostrar sucesso/erro
function showToast(title, message, iconType) {
    
    // Configura o ícone (success, error, warning, info)
    const iconColor = iconType === 'success' ? '#00c853' : (iconType === 'error' ? '#ff3d00' : '#D4AF37');

    swalDark.fire({
        title: title,
        html: message,
        icon: iconType,
        iconColor: iconColor,
        timer: 3500, // Fecha sozinho em 3.5s
        timerProgressBar: true,
        showConfirmButton: false, // Toast minimalista
        toast: true,
        position: 'top-end' // Canto superior direito
    });
}