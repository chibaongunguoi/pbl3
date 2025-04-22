function getPagination(pageNum, maxPageNum, contextUrl, contextComponent) {
    $.get("/BPController/GetPagination", { currentPage: pageNum, max_index_page: maxPageNum, context_url: contextUrl, context_component: contextComponent })
        .done(function(data) {
            $('.pagination-bar').html(data);
            attachPaginationEvents(maxPageNum, contextUrl, contextComponent)
        })
        .fail(function() {
            console.error('Error fetching pagination data');
        });
}

function getPaginationData(pageNum, contextUrl, contextComponent) {
    $.get(contextUrl, { currentPage: pageNum })
        .done(function(data) {
            $(contextComponent).html(data);
        })
        .fail(function() {
            console.error('Error fetching data');
        });
}

function attachPaginationEvents(maxPageNum, contextUrl, contextComponent) {
    const PaginationLinks = document.querySelectorAll('.page-link');
    PaginationLinks.forEach((link) => {
        const new_link = link.cloneNode(true);
        link.replaceWith(new_link)
        new_link.addEventListener('click', function() {
            const pageNum = this.dataset.index
            initPagination(pageNum, maxPageNum, contextUrl, contextComponent)
        })
    }
)}


function initPagination(pageNum, maxPageNum, contextUrl, contextComponent) {
    getPagination(pageNum, maxPageNum, contextUrl, contextComponent)
    getPaginationData(pageNum, contextUrl, contextComponent)
}