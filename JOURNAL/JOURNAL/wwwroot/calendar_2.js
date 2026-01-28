
window.showCalendarEntry = function (dateStr, Entry) {
    const placeholder = document.getElementById('Entry-placeholder');
    const content = document.getElementById('Entry-content');
    const title = document.getElementById('selected-date-title');
    const list = document.getElementById('Entry-list');

    const date = new Date(dateStr + 'T00:00:00');
    title.textContent = date.toLocaleDateString('en-US', { weekday:'long', year:'numeric', month:'long', day:'numeric' });

    list.innerHTML = '';
    Entry.forEach(e => {
        const el = document.createElement('article');
        el.className = 'rounded-md border-l-4 bg-neutral-50 p-3 hover:bg-neutral-100 dark:bg-neutral-800 dark:hover:bg-neutral-700';
        el.style.borderLeftColor = e.moodColor;
        el.onclick = () => location.href = '/entry/' + e.entryId;

        el.innerHTML = `
<h3 class="text-sm font-medium text-neutral-900 dark:text-white">${e.title}</h3>
<p class="mt-1 text-xs text-neutral-500 line-clamp-2">${e.preview}</p>
<div class="mt-2 text-[11px] text-neutral-400">${e.wordCount} words · ${e.timeString}</div>`;
        list.appendChild(el);
    });

    placeholder.classList.add('hidden');
    content.classList.remove('hidden');
};

window.closeEntryPanel = function () {
    document.getElementById('Entry-placeholder').classList.remove('hidden');
    document.getElementById('Entry-content').classList.add('hidden');
};