$(function () {
    const API_BASE = "/api"; // same origin as this page — no CORS needed

    let editingMovieId = null;
    let editingCategoryId = null;

    // ---------- Navigation between the two sections ----------
    $("#nav-movies").on("click", function (e) {
        e.preventDefault();
        $("#movies-section").show();
        $("#categories-section").hide();
        $("#nav-movies").addClass("active");
        $("#nav-categories").removeClass("active");
    });

    $("#nav-categories").on("click", function (e) {
        e.preventDefault();
        $("#categories-section").show();
        $("#movies-section").hide();
        $("#nav-categories").addClass("active");
        $("#nav-movies").removeClass("active");
    });

    // ===================== CATEGORIES =====================

    function loadCategories() {
        $.ajax({
            url: `${API_BASE}/categories`,
            method: "GET",
            success: function (categories) {
                renderCategoriesTable(categories);
                populateCategoryDropdown(categories);
            },
            error: function () {
                $("#category-form-error").text("Failed to load categories from the API.");
            }
        });
    }

    function renderCategoriesTable(categories) {
        const $tbody = $("#categories-tbody");
        $tbody.empty();
        categories.forEach(function (c) {
            $tbody.append(`
                <tr>
                    <td>${escapeHtml(c.name)}</td>
                    <td>${escapeHtml(c.code)}</td>
                    <td>
                        <button class="edit-category-btn" data-id="${c.id}">Edit</button>
                        <button class="delete-category-btn" data-id="${c.id}">Delete</button>
                    </td>
                </tr>
            `);
        });
    }

    function populateCategoryDropdown(categories) {
        const $select = $("#movie-category");
        const currentValue = $select.val();
        $select.find("option[value!='']").remove();
        categories.forEach(function (c) {
            $select.append(`<option value="${c.id}">${escapeHtml(c.name)}</option>`);
        });
        if (currentValue) $select.val(currentValue);
    }

    $("#category-form").on("submit", function (e) {
        e.preventDefault();
        $("#category-form-error").text("");

        const category = {
            id: editingCategoryId || 0,
            name: $("#category-name").val(),
            code: $("#category-code").val()
        };

        const isEdit = editingCategoryId !== null;
        $.ajax({
            url: isEdit ? `${API_BASE}/categories/${editingCategoryId}` : `${API_BASE}/categories`,
            method: isEdit ? "PUT" : "POST",
            contentType: "application/json",
            data: JSON.stringify(category),
            success: function () {
                resetCategoryForm();
                loadCategories();
            },
            error: function (xhr) {
                $("#category-form-error").text(extractErrorMessage(xhr));
            }
        });
    });

    $(document).on("click", ".edit-category-btn", function () {
        const id = $(this).data("id");
        $.get(`${API_BASE}/categories/${id}`, function (c) {
            editingCategoryId = c.id;
            $("#category-id").val(c.id);
            $("#category-name").val(c.name);
            $("#category-code").val(c.code);
            $("#category-form-title").text("Edit Category");
            $("#category-submit-btn").text("Save");
            $("#category-cancel-btn").show();
        });
    });

    $("#category-cancel-btn").on("click", resetCategoryForm);

    function resetCategoryForm() {
        editingCategoryId = null;
        $("#category-form")[0].reset();
        $("#category-form-title").text("Create New Category");
        $("#category-submit-btn").text("Create");
        $("#category-cancel-btn").hide();
        $("#category-form-error").text("");
    }

    $(document).on("click", ".delete-category-btn", function () {
        const id = $(this).data("id");
        if (!confirm("Delete this category?")) return;

        $.ajax({
            url: `${API_BASE}/categories/${id}`,
            method: "DELETE",
            success: function () {
                loadCategories();
            },
            error: function (xhr) {
                alert(extractErrorMessage(xhr));
            }
        });
    });

    // ===================== MOVIES =====================

    function loadMovies() {
        $.ajax({
            url: `${API_BASE}/movies`,
            method: "GET",
            success: function (movies) {
                renderMoviesTable(movies);
            },
            error: function () {
                $("#movie-form-error").text("Failed to load movies from the API.");
            }
        });
    }

    function renderMoviesTable(movies) {
        const $tbody = $("#movies-tbody");
        $tbody.empty();
        movies.forEach(function (m) {
            $tbody.append(`
                <tr>
                    <td>${escapeHtml(m.name)}</td>
                    <td>${formatDate(m.releaseDate)}</td>
                    <td>${escapeHtml(m.director)}</td>
                    <td>${escapeHtml(m.contactEmail)}</td>
                    <td>${escapeHtml(m.language)}</td>
                    <td>${escapeHtml(m.categoryName || "")}</td>
                    <td>
                        <button class="edit-movie-btn" data-id="${m.id}">Edit</button>
                        <button class="delete-movie-btn" data-id="${m.id}">Delete</button>
                    </td>
                </tr>
            `);
        });
    }

    $("#movie-form").on("submit", function (e) {
        e.preventDefault();
        $("#movie-form-error").text("");

        const movie = {
            id: editingMovieId || 0,
            name: $("#movie-name").val(),
            releaseDate: $("#movie-release-date").val(),
            director: $("#movie-director").val(),
            contactEmail: $("#movie-email").val(),
            language: $("#movie-language").val(),
            categoryId: parseInt($("#movie-category").val(), 10)
        };

        const isEdit = editingMovieId !== null;
        $.ajax({
            url: isEdit ? `${API_BASE}/movies/${editingMovieId}` : `${API_BASE}/movies`,
            method: isEdit ? "PUT" : "POST",
            contentType: "application/json",
            data: JSON.stringify(movie),
            success: function () {
                resetMovieForm();
                loadMovies();
            },
            error: function (xhr) {
                $("#movie-form-error").text(extractErrorMessage(xhr));
            }
        });
    });

    $(document).on("click", ".edit-movie-btn", function () {
        const id = $(this).data("id");
        $.get(`${API_BASE}/movies/${id}`, function (m) {
            editingMovieId = m.id;
            $("#movie-id").val(m.id);
            $("#movie-name").val(m.name);
            $("#movie-release-date").val(m.releaseDate.split("T")[0]);
            $("#movie-director").val(m.director);
            $("#movie-email").val(m.contactEmail);
            $("#movie-language").val(m.language);
            $("#movie-category").val(m.categoryId);
            $("#movie-form-title").text("Edit Movie");
            $("#movie-submit-btn").text("Save");
            $("#movie-cancel-btn").show();
        });
    });

    $("#movie-cancel-btn").on("click", resetMovieForm);

    function resetMovieForm() {
        editingMovieId = null;
        $("#movie-form")[0].reset();
        $("#movie-form-title").text("Create New Movie");
        $("#movie-submit-btn").text("Create");
        $("#movie-cancel-btn").hide();
        $("#movie-form-error").text("");
    }

    $(document).on("click", ".delete-movie-btn", function () {
        const id = $(this).data("id");
        if (!confirm("Delete this movie?")) return;

        $.ajax({
            url: `${API_BASE}/movies/${id}`,
            method: "DELETE",
            success: function () {
                loadMovies();
            },
            error: function (xhr) {
                alert(extractErrorMessage(xhr));
            }
        });
    });

    // ---------- Helpers ----------
    function escapeHtml(str) {
        if (str === null || str === undefined) return "";
        return String(str)
            .replace(/&/g, "&amp;")
            .replace(/</g, "&lt;")
            .replace(/>/g, "&gt;");
    }

    function formatDate(isoString) {
        const d = new Date(isoString);
        return d.toLocaleDateString();
    }

    function extractErrorMessage(xhr) {
        try {
            const body = JSON.parse(xhr.responseText);
            if (body.error) return body.error;
            if (body.title) return body.title; // ASP.NET Core validation problem details
            // ModelState errors come back as { field: ["msg"] } — flatten the first one
            const firstKey = Object.keys(body)[0];
            if (firstKey && Array.isArray(body[firstKey])) return body[firstKey][0];
        } catch (e) {
            // fall through
        }
        return "Something went wrong (status " + xhr.status + ").";
    }

    // ---------- Initial load ----------
    loadCategories();
    loadMovies();
});
