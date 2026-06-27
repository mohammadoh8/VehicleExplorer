class SearchDropdown {
    constructor(element, options) {
        this.element = element;
        this.options = options;
        this.items = [];
        this.page = 1;
        this.totalCount = 0;
        this.search = "";
        this.selected = null;
        this.isLoading = false;

        this.render();
        this.bindEvents();
        this.load();
    }

    render() {
        this.element.classList.add("position-relative");
        this.element.innerHTML = `
            <div class="input-group">
                <input id="${this.options.inputId}" class="form-control" type="text" autocomplete="off" placeholder="${this.options.placeholder}" />
                <button class="btn btn-outline-secondary search-dropdown__clear" type="button">Clear</button>
            </div>
            <div class="dropdown-menu w-100 p-0 shadow-sm overflow-auto search-dropdown__menu" style="max-height: 280px;">
                <div class="search-dropdown__items"></div>
                <button class="dropdown-item text-primary fw-semibold border-top search-dropdown__more" type="button">Load more</button>
            </div>
        `;

        this.input = this.element.querySelector(".form-control");
        this.clearButton = this.element.querySelector(".search-dropdown__clear");
        this.menu = this.element.querySelector(".search-dropdown__menu");
        this.itemsContainer = this.element.querySelector(".search-dropdown__items");
        this.moreButton = this.element.querySelector(".search-dropdown__more");
    }

    bindEvents() {
        let debounceId;

        this.input.addEventListener("focus", () => this.open());
        this.input.addEventListener("input", () => {
            clearTimeout(debounceId);
            debounceId = setTimeout(() => {
                this.selected = null;
                this.search = this.input.value.trim();
                this.page = 1;
                this.load();
                this.options.onChange?.(null);
            }, 250);
        });

        this.clearButton.addEventListener("click", () => {
            this.selected = null;
            this.search = "";
            this.input.value = "";
            this.page = 1;
            this.load();
            this.options.onChange?.(null);
            this.open();
        });

        this.moreButton.addEventListener("click", () => {
            const pageSize = this.options.pageSize ?? 10;
            this.page = Math.floor(this.items.length / pageSize) + 1;
            this.load(true);
        });

        document.addEventListener("click", event => {
            if (!this.element.contains(event.target)) {
                this.close();
            }
        });
    }

    async load(append = false) {
        if (this.isLoading) {
            return;
        }

        this.isLoading = true;
        this.moreButton.disabled = true;
        this.setStatus("Loading...");

        try {
            const result = this.options.mode === "remote"
                ? await this.loadRemote()
                : this.loadLocal(append);

            this.items = append ? [...this.items, ...result.items] : result.items;
            this.totalCount = result.totalCount;
            this.drawItems();
        } catch {
            this.items = [];
            this.totalCount = 0;
            this.setStatus("Unable to load options.");
            this.moreButton.hidden = true;
        } finally {
            this.isLoading = false;
            this.moreButton.disabled = false;
        }
    }

    async loadRemote() {
        const url = new URL(this.options.url, window.location.origin);
        url.searchParams.set("page", this.page);
        url.searchParams.set("pageSize", this.options.pageSize ?? 10);

        if (this.search) {
            url.searchParams.set("search", this.search);
        }

        const response = await fetch(url);
        const body = await response.json();

        if (!body.success) {
            throw new Error(body.message ?? "Request failed");
        }

        return {
            items: body.data.items,
            totalCount: body.data.totalCount
        };
    }

    loadLocal(append) {
        const allItems = this.options.getItems?.() ?? [];
        const filtered = this.search
            ? allItems.filter(item => item.name.toLowerCase().includes(this.search.toLowerCase()))
            : allItems;

        const pageSize = this.options.pageSize ?? 10;
        const start = append ? (this.page - 1) * pageSize : 0;
        const end = this.page * pageSize;

        return {
            items: filtered.slice(start, end),
            totalCount: filtered.length
        };
    }

    drawItems() {
        if (this.items.length === 0) {
            this.setStatus("No options found.");
            this.moreButton.hidden = true;
            return;
        }

        this.itemsContainer.innerHTML = "";

        for (const item of this.items) {
            const button = document.createElement("button");
            button.type = "button";
            button.className = "dropdown-item text-wrap";
            button.textContent = item.name;
            button.addEventListener("click", () => this.select(item));
            this.itemsContainer.appendChild(button);
        }

        this.moreButton.hidden = this.items.length >= this.totalCount;
    }

    select(item) {
        this.selected = item;
        this.input.value = item.name;
        this.options.onChange?.(item);
        this.close();
    }

    setItems(items) {
        this.options.getItems = () => items;
        this.selected = null;
        this.search = "";
        this.input.value = "";
        this.page = 1;
        this.load();
    }

    setStatus(message) {
        this.itemsContainer.innerHTML = `<div class="px-3 py-2 text-muted small">${message}</div>`;
    }

    open() {
        this.menu.classList.add("show");
    }

    close() {
        this.menu.classList.remove("show");
    }
}

document.addEventListener("DOMContentLoaded", () => {
    const makeDropdownElement = document.getElementById("makeDropdown");
    const typeDropdownElement = document.getElementById("typeDropdown");
    const message = document.getElementById("message");
    const yearInput = document.getElementById("modelYear");
    const modelsList = document.getElementById("modelsList");
    const modelCount = document.getElementById("modelCount");
    const modelSearchInput = document.getElementById("modelSearchInput");
    const modelsPagination = document.getElementById("modelsPagination");
    const searchButton = document.getElementById("searchModelsButton");

    if (
        !makeDropdownElement ||
        !typeDropdownElement ||
        !message ||
        !yearInput ||
        !modelsList ||
        !modelCount ||
        !modelSearchInput ||
        !modelsPagination ||
        !searchButton
    ) {
        return;
    }

    let selectedMake = null;
    let selectedType = null;
    let allModels = [];
    let filteredModels = [];
    let modelsPage = 1;
    const modelsPageSize = 8;

    const showMessage = (text, type = "info") => {
        if (!text) {
            message.textContent = "";
            message.className = "alert d-none";
            return;
        }

        message.textContent = text;
        message.className = type === "error"
            ? "alert alert-danger"
            : "alert alert-info";
    };

    const clearModels = () => {
        modelsList.innerHTML = "";
        modelCount.textContent = "";
        modelsPagination.innerHTML = "";
        modelSearchInput.value = "";
        modelSearchInput.disabled = true;
        allModels = [];
        filteredModels = [];
        modelsPage = 1;
    };

    const typeDropdown = new SearchDropdown(typeDropdownElement, {
        mode: "local",
        inputId: "typeDropdownInput",
        placeholder: "Select a make first",
        pageSize: 8,
        getItems: () => [],
        onChange: item => {
            selectedType = item;
            clearModels();
        }
    });

    new SearchDropdown(makeDropdownElement, {
        mode: "remote",
        inputId: "makeDropdownInput",
        placeholder: "Search make",
        url: "/Vehicles/Makes",
        pageSize: 10,
        onChange: async item => {
            selectedMake = item;
            selectedType = null;
            clearModels();

            if (!item) {
                typeDropdown.setItems([]);
                showMessage("");
                return;
            }

            await loadVehicleTypes(item.id);
        }
    });

    async function loadVehicleTypes(makeId) {
        showMessage("Loading vehicle types...");

        try {
            const response = await fetch(`/Vehicles/Types?makeId=${encodeURIComponent(makeId)}`);
            const body = await response.json();

            if (!body.success) {
                typeDropdown.setItems([]);
                showMessage(body.message ?? "Unable to load vehicle types.", "error");
                return;
            }

            typeDropdown.setItems(body.data);

            if (body.data.length === 0) {
                showMessage("No vehicle types found for this make.");
                return;
            }

            showMessage("Select a vehicle type to continue.");
        } catch {
            typeDropdown.setItems([]);
            showMessage("Unable to load vehicle types.", "error");
        }
    }

    async function loadModels() {
        clearModels();

        if (!selectedMake) {
            showMessage("Select a make first.", "error");
            return;
        }

        if (!selectedType) {
            showMessage("Select a vehicle type.", "error");
            return;
        }

        const year = Number(yearInput.value);

        if (!year) {
            showMessage("Enter a model year.", "error");
            return;
        }

        searchButton.disabled = true;
        showMessage("Loading models...");

        const url = new URL("/Vehicles/Models", window.location.origin);
        url.searchParams.set("makeId", selectedMake.id);
        url.searchParams.set("year", year);
        url.searchParams.set("vehicleType", selectedType.name);

        try {
            const response = await fetch(url);
            const body = await response.json();

            if (!body.success) {
                showMessage(body.message ?? "Unable to load models.", "error");
                return;
            }

            allModels = body.data;
            modelSearchInput.disabled = allModels.length === 0;
            applyModelFilter();
        } catch {
            showMessage("Unable to load models.", "error");
        } finally {
            searchButton.disabled = false;
        }
    }

    function applyModelFilter() {
        const search = modelSearchInput.value.trim().toLowerCase();

        filteredModels = search
            ? allModels.filter(model => model.name.toLowerCase().includes(search))
            : [...allModels];

        modelsPage = 1;
        renderModels();
    }

    function renderModels() {
        if (filteredModels.length === 0) {
            modelCount.textContent = "0 found";
            modelsList.innerHTML = "";
            modelsPagination.innerHTML = "";
            showMessage("No models found for this selection.");
            return;
        }

        const totalPages = Math.ceil(filteredModels.length / modelsPageSize);
        const start = (modelsPage - 1) * modelsPageSize;
        const pageItems = filteredModels.slice(start, start + modelsPageSize);

        modelCount.textContent = `${filteredModels.length} found`;
        showMessage("");

        modelsList.innerHTML = pageItems
            .map(model => `<div class="list-group-item">${escapeHtml(model.name)}</div>`)
            .join("");

        renderModelsPagination(totalPages);
    }

    function renderModelsPagination(totalPages) {
        modelsPagination.innerHTML = "";

        if (totalPages <= 1) {
            return;
        }

        const createPageItem = (label, page, disabled = false, active = false) => {
            const item = document.createElement("li");
            item.className = `page-item${disabled ? " disabled" : ""}${active ? " active" : ""}`;

            const button = document.createElement("button");
            button.className = "page-link";
            button.type = "button";
            button.textContent = label;
            button.disabled = disabled;
            button.addEventListener("click", () => {
                modelsPage = page;
                renderModels();
            });

            item.appendChild(button);
            return item;
        };

        modelsPagination.appendChild(createPageItem("Previous", modelsPage - 1, modelsPage === 1));

        for (let page = 1; page <= totalPages; page += 1) {
            modelsPagination.appendChild(createPageItem(String(page), page, false, page === modelsPage));
        }

        modelsPagination.appendChild(createPageItem("Next", modelsPage + 1, modelsPage === totalPages));
    }

    function escapeHtml(value) {
        return value
            .replaceAll("&", "&amp;")
            .replaceAll("<", "&lt;")
            .replaceAll(">", "&gt;")
            .replaceAll('"', "&quot;")
            .replaceAll("'", "&#039;");
    }

    searchButton.addEventListener("click", loadModels);
    modelSearchInput.addEventListener("input", applyModelFilter);
});
