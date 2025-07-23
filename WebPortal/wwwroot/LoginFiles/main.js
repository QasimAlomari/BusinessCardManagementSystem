const toggleBtn = document.getElementById('themeToggleBtn');
const icon = toggleBtn.querySelector('i');
const body = document.body;

toggleBtn.addEventListener('click', () => {
    body.classList.toggle('dark');

    if (body.classList.contains('dark')) {
        icon.classList.remove('fa-moon');
        icon.classList.add('fa-sun');
    } else {
        icon.classList.remove('fa-sun');
        icon.classList.add('fa-moon');
    }
});