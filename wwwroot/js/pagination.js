function getPagination(paginationInfo, contextUrl, contextComponent, filterForm, paginationBar) {
    const sentData = { ...paginationInfo, ...filterForm, contextUrl: contextUrl, contextComponent: contextComponent }
    $.get(`${contextUrl}/Pagination`, sentData)
        .done(function (data) {
            $(paginationBar).html(data);
            attachPaginationEvents(paginationInfo, contextUrl, contextComponent, filterForm, paginationBar)
        })
        .fail(function () {
            console.error('Error fetching pagination data');
        });
}

function getPaginationData(paginationInfo, contextUrl, contextComponent, filterForm) {
    $(contextComponent).html(
        '<div class="d-flex justify-content-center"><div class="spinner-border" role="status"><span class="visually-hidden">Loading...</span></div></div>'
    )
    const sentData = { ...paginationInfo, ...filterForm }
    $.get(contextUrl, sentData)
        .done(function (data) {
            $(contextComponent).html(data);
        })
        .fail(function () {
            console.error('Error fetching data');
        });
}

function attachPaginationEvents(paginationInfo, contextUrl, contextComponent, filterForm, paginationBar) {
    const PaginationLinks = document.querySelectorAll('.page-link');
    PaginationLinks.forEach((link) => {
        const new_link = link.cloneNode(true);
        link.replaceWith(new_link)
        new_link.addEventListener('click', function () {
            paginationInfo["CurrentPage"] = this.dataset.index
            initPagination(paginationInfo, contextUrl, contextComponent, filterForm, paginationBar, null)
        })
    }
    )
}


function initPagination(paginationInfo, contextUrl, contextComponent, filterForm, paginationBar, paginationForm = null) {
    getPagination(paginationInfo, contextUrl, contextComponent, filterForm, paginationBar)
    getPaginationData(paginationInfo, contextUrl, contextComponent, filterForm)

    if (paginationForm != null) {
        const formContainer = document.querySelector(paginationForm);
        const form = formContainer.querySelector('form');
        form.addEventListener('submit', function (event) {
            event.preventDefault()
            const filterForm = Object.fromEntries(new FormData(form).entries())
            paginationInfo["CurrentPage"] = 1;
            initPagination(paginationInfo, contextUrl, contextComponent, filterForm, paginationBar, null)
        });
    }
}