window.getScrollHeight = function(element) {
    if (!element || typeof element.scrollHeight !== "number") {
        return 0; // Ensure a fallback value
    }
    return Math.floor(element.scrollHeight); // Ensure it's an integer
};

window.getClientHeight = function(element) {
    if (!element || typeof element.clientHeight !== "number") {
        return 0; // Ensure a fallback value
    }
    return Math.floor(element.clientHeight); // Ensure it's an integer
};

window.getScrollTop = function(element) {
    if (!element || typeof element.scrollTop !== "number") {
        return 0; // Ensure a fallback value
    }
    return Math.floor(element.scrollTop); // Ensure it's an integer
};

// Add this to your JavaScript file or add a script tag to your _Host.cshtml
window.closeDropdown = function (buttonId) {
    var dropdownToggle = document.getElementById(buttonId);
    if (dropdownToggle) {
        var dropdown = bootstrap.Dropdown.getInstance(dropdownToggle);
        if (dropdown) {
            dropdown.hide();
        }
    }
};
