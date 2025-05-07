function getPagination(pageNum, maxPageNum, contextUrl, contextComponent, filterForm) {
    $.get("/BPController/GetPagination", { currentPage: pageNum, totalPages: maxPageNum, contextUrl: contextUrl, contextComponent: contextComponent })
        .done(function(data) {
            $('.pagination-bar').html(data);
            attachPaginationEvents(maxPageNum, contextUrl, contextComponent, filterForm)
        })
        .fail(function() {
            console.error('Error fetching pagination data');
        });
}

function getPaginationData(pageNum, contextUrl, contextComponent, filterForm) {
    $(contextComponent).html(
        '<div class="d-flex justify-content-center"><div class="spinner-border" role="status"><span class="visually-hidden">Loading...</span></div></div>'
    )
    filterForm['currentPage'] = pageNum
    $.get(contextUrl, filterForm)
        .done(function(data) {
            $(contextComponent).html(data);
        })
        .fail(function() {
            console.error('Error fetching data');
        });
}

function attachPaginationEvents(maxPageNum, contextUrl, contextComponent, filterForm) {
    const PaginationLinks = document.querySelectorAll('.page-link');
    PaginationLinks.forEach((link) => {
        const new_link = link.cloneNode(true);
        link.replaceWith(new_link)
        new_link.addEventListener('click', function() {
            const pageNum = this.dataset.index
            initPagination(pageNum, maxPageNum, contextUrl, contextComponent, filterForm)
        })
    }
)}


function initPagination(pageNum, maxPageNum, contextUrl, contextComponent, filterForm) {
    getPagination(pageNum, maxPageNum, contextUrl, contextComponent, filterForm)
    getPaginationData(pageNum, contextUrl, contextComponent, filterForm)
}