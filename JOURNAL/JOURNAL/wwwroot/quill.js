let currentQuill = null;

window.initQuill = (editorId) => {
    try {
        const el = document.getElementById(editorId);
        if (!el) return false;

        currentQuill = new Quill(`#${editorId}`, {
            theme: "snow",
            modules: {
                toolbar: [
                    ["bold", "italic", "underline"],
                    [{ list: "ordered" }, { list: "bullet" }],
                    [{ header: [1, 2, false] }],
                    ["clean"]
                ]
            },
            placeholder: "Write your thoughts here..."
        });

        return true;
    } catch {
        return false;
    }
};

window.getQuillHtml = () => currentQuill ? currentQuill.root.innerHTML : "";

window.setQuillHtml = (html) => {
    if (!currentQuill) return false;
    currentQuill.root.innerHTML = html;
    return true;
};