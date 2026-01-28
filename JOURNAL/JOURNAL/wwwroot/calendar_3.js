function initJournalCalendar(containerId, events, detailsContainerId, noSelectionMessageId) {
    const container = document.getElementById(containerId);
    if (!container) return;

    // Initialize FullCalendar
    const calendarEl = document.createElement('div');
    calendarEl.id = 'calendar';
    container.appendChild(calendarEl);

    const calendar = new FullCalendar.Calendar(calendarEl, {
        initialView: 'dayGridMonth',
        headerToolbar: {
            left: 'prev,next today',
            center: 'title',
            right: 'dayGridMonth,dayGridWeek,dayGridDay'
        },
        events: events.map(event => ({
            title: event.title,
            start: event.start,
            backgroundColor: getEntryColor(event.content),
            borderColor: '#d4c4a8',
            textColor: '#5c4b37',
            extendedProps: {
                entryId: event.entryId,
                content: event.content,
                createdAt: event.createdAt,
                wordCount: event.wordCount
            }
        })),
        eventClick: function(info) {
            showEntryDetails(info.event.extendedProps, detailsContainerId, noSelectionMessageId);
        },
        dayCellDidMount: function(info) {
            // Add subtle styling to days with entries
            const hasEvent = info.date.getDate() === new Date().getDate() &&
                info.date.getMonth() === new Date().getMonth();
            if (hasEvent) {
                info.el.style.backgroundColor = '#f8f4e8';
            }
        },
        themeSystem: 'standard',
        height: 'auto',
        fixedWeekCount: false,
        dayMaxEvents: true,
        views: {
            dayGridMonth: {
                dayMaxEventRows: 2
            }
        }
    });

    calendar.render();

    // Function to get color based on content tone
    function getEntryColor(content) {
        const text = content.toLowerCase().replace(/<[^>]*>/g, '');
        const positiveWords = ['happy', 'joy', 'grateful', 'excited', 'love', 'good', 'great'];
        const negativeWords = ['sad', 'angry', 'frustrated', 'bad', 'worried', 'anxious'];

        let positiveCount = positiveWords.filter(word => text.includes(word)).length;
        let negativeCount = negativeWords.filter(word => text.includes(word)).length;

        if (positiveCount > negativeCount) return '#d4a574'; // Reflective
        if (negativeCount > positiveCount) return '#8b7b65'; // Thoughtful
        return '#c4a484'; // Neutral
    }

    // Function to show entry details in sidebar
    function showEntryDetails(entry, detailsContainerId, noSelectionMessageId) {
        const detailsContainer = document.getElementById(detailsContainerId);
        const noSelectionMsg = document.getElementById(noSelectionMessageId);

        if (!detailsContainer) return;

        // Hide "no selection" message
        if (noSelectionMsg) {
            noSelectionMsg.style.display = 'none';
        }

        // Format date
        const date = new Date(entry.createdAt);
        const dateStr = date.toLocaleDateString('en-US', {
            weekday: 'long',
            year: 'numeric',
            month: 'long',
            day: 'numeric'
        });
        const timeStr = date.toLocaleTimeString('en-US', {
            hour: '2-digit',
            minute: '2-digit'
        });

        // Clean HTML content
        const cleanContent = entry.content.replace(/<[^>]*>/g, '');
        const previewContent = cleanContent.length > 200 ?
            cleanContent.substring(0, 200) + '...' : cleanContent;

        // Create HTML for entry details
        detailsContainer.innerHTML = `
            <div class="space-y-4">
                <div>
                    <h3 class="text-lg font-medium text-blue mb-1">${entry.title || 'Untitled Notes'}</h3>
                    <div class="flex items-center gap-2 text-sm text-blue">
                        <svg class="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="1.5" d="M8 7V3m8 4V3m-9 8h10M5 21h14a2 2 0 002-2V7a2 2 0 00-2-2H5a2 2 0 00-2 2v12a2 2 0 002 2z" />
                        </svg>
                        <span>${dateStr}</span>
                    </div>
                </div>
                
                <div class="border border-blue rounded-lg p-4 ">
                    <h4 class="text-sm text-blue uppercase tracking-wider mb-2">Reading Notes</h4>
                    <p class="text-blue leading-relaxed">${previewContent}</p>
                    <div class="mt-4 pt-3 border-t border-blue">
                        <a href="/entry/${entry.entryId}" 
                           class="text-sm text-blue hover:text-blue flex items-center gap-1">
                            Read full notes
                            <svg class="w-3 h-3" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 5l7 7-7 7" />
                            </svg>
                        </a>
                    </div>
                </div>
                
                <div class="grid grid-cols-2 gap-3 text-sm">
                    <div class="text-center p-2 border border-blue rounded">
                        <p class="text-blue">Words</p>
                        <p class="text-blue font-medium">${entry.wordCount}</p>
                    </div>
                    <div class="text-center p-2 border border-blue rounded">
                        <p class="text-blue">Time</p>
                        <p class="text-blue font-medium">${timeStr}</p>
                    </div>
                </div>
            </div>
        `;
    }
}
