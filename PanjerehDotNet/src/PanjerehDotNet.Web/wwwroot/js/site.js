document.addEventListener('DOMContentLoaded', () => {
    const btn = document.getElementById('themeToggle');
    btn.onclick = () => {
        document.body.classList.toggle('dark');
        document.body.classList.toggle('light');
    };
});
