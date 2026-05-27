document.addEventListener('DOMContentLoaded', function () {
    document.querySelectorAll('#toast-container .toast').forEach(function (el) {
        bootstrap.Toast.getOrCreateInstance(el).show();
    });
});

window.showToast = function (message, type) {
    type = type || 'success';
    var cfg = {
        success: { icon: 'bi-check-circle-fill',         cls: 'text-success', border: '#198754', title: 'Éxito',       style: '' },
        danger:  { icon: 'bi-x-circle-fill',             cls: 'text-danger',  border: '#dc3545', title: 'Error',       style: '' },
        warning: { icon: 'bi-exclamation-triangle-fill', cls: 'text-warning', border: '#ffc107', title: 'Advertencia', style: '' },
        info:    { icon: 'bi-info-circle-fill',          cls: '',             border: '#6f42c1', title: 'Información', style: 'color:#6f42c1;' }
    };
    var c = cfg[type] || cfg.success;
    var container = document.getElementById('toast-container');
    if (!container) return;

    var el = document.createElement('div');
    el.className = 'toast toast-notaria toast-' + type;
    el.setAttribute('role', 'alert');
    el.setAttribute('data-bs-autohide', 'true');
    el.setAttribute('data-bs-delay', type === 'danger' ? '7000' : '5000');
    el.innerHTML =
        '<div class="d-flex align-items-center p-3">' +
            '<div class="me-3"><i class="bi ' + c.icon + ' fs-5 ' + c.cls + '" style="' + c.style + '"></i></div>' +
            '<div class="flex-grow-1">' +
                '<div class="fw-semibold ' + c.cls + ' small mb-1" style="' + c.style + '">' + c.title + '</div>' +
                '<div class="text-secondary small">' + message + '</div>' +
            '</div>' +
            '<button type="button" class="btn-close btn-close-sm ms-2 flex-shrink-0" data-bs-dismiss="toast" aria-label="Cerrar"></button>' +
        '</div>';

    container.appendChild(el);
    var toast = bootstrap.Toast.getOrCreateInstance(el);
    toast.show();
    el.addEventListener('hidden.bs.toast', function () { el.remove(); });
};
