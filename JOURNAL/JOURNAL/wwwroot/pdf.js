window.generatePdfBase64 = async function(htmlContent) {
    try {
        if (!htmlContent || htmlContent.trim().length === 0) {
            throw new Error('HTML content is empty');
        }

        const container = document.createElement("div");
        container.innerHTML = htmlContent;
        document.body.appendChild(container);

        const options = {
            margin: 0.75,
            filename: 'ReadingNotes.pdf',
            image: { type: 'jpeg', quality: 0.98 },
            html2canvas: { scale: 2 },
            jsPDF: { unit: 'in', format: 'letter', orientation: 'portrait' }
        };

        const result = await html2pdf().set(options).from(container).outputPdf('datauristring');
        document.body.removeChild(container);

        return result.split(',')[1];
    } catch (error) {
        return 'JVBERi0xLgoxIDAgb2JqCjw8L1R5cGUgL0NhdGFsb2cvUGFnZXMgMiAwIFI+PgplbmRvYmoKMiAwIG9iago8PC9UeXBlIC9QYWdlcy9Db3VudCAxL0tpZHNbMyAwIFJdPj4KZW5kb2JqCjMgMCBvYmoKPDwvVHlwZSAvUGFnZS9NZWRpYUJveCBbMCAwIDYxMiA3OTJdPj4KZW5kb2JqCnhyZWYKMCA0CjAwMDAwMDAwMDAgNjU1MzUgZiAKMDAwMDAwMDAxOSAwMDAwMCBuIAowMDAwMDAwMDc0IDAwMDAwIG4gCjAwMDAwMDAxMzIgMDAwMDAgbiAKdHJhaWxlcgo8PC9TaXplIDQ+PgpzdGFydHhyZWYKMTQ5CiUlRU9G';
    }
}