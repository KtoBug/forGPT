document.addEventListener('DOMContentLoaded', function () {
    const chairmanBtn = document.getElementById('btn-chairman');
    if (chairmanBtn) {
        chairmanBtn.addEventListener('click', function () {
            const password = prompt("Введите пароль администратора:");
            if (!password) return;

            fetch('/api/Admin/VerifyPassword', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/x-www-form-urlencoded',
                },
                body: `password=${encodeURIComponent(password)}`
            })
            .then(response => {
                if (response.ok) return response.json();
                throw new Error("Ошибка проверки");
            })
            .then(data => {
                if (data.success) {
                    const panel = document.getElementById('admin-panel');
                    if (panel) panel.style.display = 'block';
                    document.querySelectorAll('.delete-button').forEach(btn => btn.style.display = 'inline-block');
                    window.scrollTo(0, document.body.scrollHeight);
                } else {
                    alert(data.message || "Неверный пароль");
                }
            })
            .catch(err => alert(err.message));
        });
    }

    document.querySelectorAll('.delete-button').forEach(button => {
        button.addEventListener('click', function (e) {
            if (!confirm("Вы уверены, что хотите удалить?")) e.preventDefault();
        });
    });
});
