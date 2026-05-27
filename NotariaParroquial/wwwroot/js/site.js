document.addEventListener('DOMContentLoaded', function () {
    document.querySelectorAll('#toast-container .toast').forEach(function (el) {
        bootstrap.Toast.getOrCreateInstance(el).show();
    });
});

window.showToast = function (message, type) {
    type = type || 'success';
    var cfg = {
        success: { icon: 'bi-check-circle-fill', cls: 'text-success', style: '' },
        danger:  { icon: 'bi-x-circle-fill',     cls: 'text-danger',  style: '' },
        warning: { icon: 'bi-exclamation-triangle-fill', cls: 'text-warning', style: '' },
        info:    { icon: 'bi-info-circle-fill',   cls: '',             style: 'color:#6f42c1;' }
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
        '<i class="bi ' + c.icon + ' ' + c.cls + ' me-2 fs-5"' + (c.style ? ' style="' + c.style + '"' : '') + '></i>' +
        '<div class="flex-grow-1">' + message + '</div>' +
        '<button type="button" class="btn-close ms-2" data-bs-dismiss="toast"></button>' +
        '</div>';
    container.appendChild(el);
    var toast = bootstrap.Toast.getOrCreateInstance(el);
    toast.show();
    el.addEventListener('hidden.bs.toast', function () { el.remove(); });
};
