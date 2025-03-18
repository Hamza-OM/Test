/**
 * Main JavaScript file for Product Management System
 */

// Document ready function
$(document).ready(function() {
    
    // Initialize form validation
    initializeFormValidation();
    
    // Initialize delete confirmation dialogs
    initializeDeleteConfirmation();
    
    // Initialize search functionality
    initializeSearch();
});

/**
 * Initializes form validation for product and category forms
 */
function initializeFormValidation() {
    // Product form validation
    $('#productForm').submit(function(e) {
        var isValid = true;
        
        // Validate product name
        if ($('#productName').val().trim() === '') {
            showError('#productName', 'Product name is required');
            isValid = false;
        } else {
            removeError('#productName');
        }
        
        // Validate category selection
        if ($('#categoryID').val() === '0') {
            showError('#categoryID', 'Please select a category');
            isValid = false;
        } else {
            removeError('#categoryID');
        }
        
        // Validate unit price
        var unitPrice = $('#unitPrice').val().trim();
        if (unitPrice === '' || isNaN(unitPrice) || parseFloat(unitPrice) < 0) {
            showError('#unitPrice', 'Please enter a valid price');
            isValid = false;
        } else {
            removeError('#unitPrice');
        }
        
        // Validate units in stock
        var unitsInStock = $('#unitsInStock').val().trim();
        if (unitsInStock === '' || isNaN(unitsInStock) || parseInt(unitsInStock) < 0) {
            showError('#unitsInStock', 'Please enter a valid quantity');
            isValid = false;
        } else {
            removeError('#unitsInStock');
        }
        
        // Prevent form submission if validation fails
        if (!isValid) {
            e.preventDefault();
        }
    });
    
    // Category form validation
    $('#categoryForm').submit(function(e) {
        var isValid = true;
        
        // Validate category name
        if ($('#categoryName').val().trim() === '') {
            showError('#categoryName', 'Category name is required');
            isValid = false;
        } else {
            removeError('#categoryName');
        }
        
        // Prevent form submission if validation fails
        if (!isValid) {
            e.preventDefault();
        }
    });
}

/**
 * Shows validation error message for a form field
 */
function showError(fieldSelector, message) {
    var field = $(fieldSelector);
    field.addClass('error');
    
    // Create error message if it doesn't exist
    if (field.next('.error-message').length === 0) {
        field.after('<span class="error-message">' + message + '</span>');
    } else {
        field.next('.error-message').text(message);
    }
}

/**
 * Removes validation error for a form field
 */
function removeError(fieldSelector) {
    var field = $(fieldSelector);
    field.removeClass('error');
    field.next('.error-message').remove();
}

/**
 * Initializes delete confirmation dialogs
 */
function initializeDeleteConfirmation() {
    $('.delete-btn').click(function(e) {
        if (!confirm('Are you sure you want to delete this item? This action cannot be undone.')) {
            e.preventDefault();
        }
    });
}

/**
 * Initializes live search functionality
 */
function initializeSearch() {
    $('#searchInput').on('keyup', function() {
        var searchTerm = $(this).val().toLowerCase();
        
        // Filter products table
        $('.product-row').each(function() {
            var productName = $(this).find('.product-name').text().toLowerCase();
            var categoryName = $(this).find('.category-name').text().toLowerCase();
            var description = $(this).find('.description').text().toLowerCase();
            
            if (productName.indexOf(searchTerm) > -1 || 
                categoryName.indexOf(searchTerm) > -1 || 
                description.indexOf(searchTerm) > -1) {
                $(this).show();
            } else {
                $(this).hide();
            }
        });
        
        // Filter categories table
        $('.category-row').each(function() {
            var categoryName = $(this).find('.category-name').text().toLowerCase();
            var description = $(this).find('.description').text().toLowerCase();
            
            if (categoryName.indexOf(searchTerm) > -1 || description.indexOf(searchTerm) > -1) {
                $(this).show();
            } else {
                $(this).hide();
            }
        });
        
        // Show empty message if no results
        if ($('.product-row:visible').length === 0) {
            if ($('#productEmptyMessage').length === 0) {
                $('#productTable').append('<tr id="productEmptyMessage"><td colspan="6" class="empty-message">No products found matching your search.</td></tr>');
            }
        } else {
            $('#productEmptyMessage').remove();
        }
        
        if ($('.category-row:visible').length === 0) {
            if ($('#categoryEmptyMessage').length === 0) {
                $('#categoryTable').append('<tr id="categoryEmptyMessage"><td colspan="3" class="empty-message">No categories found matching your search.</td></tr>');
            }
        } else {
            $('#categoryEmptyMessage').remove();
        }
    });
}

/**
 * Updates the units in stock value for a product (AJAX)
 */
function updateStock(productID, newValue) {
    // Ensure value is a positive integer
    newValue = parseInt(newValue);
    if (isNaN(newValue) || newValue < 0) {
        alert('Please enter a valid quantity');
        return;
    }
    
    // Send AJAX request to update stock
    $.ajax({
        type: 'POST',
        url: 'UpdateStock.aspx',
        data: {
            productID: productID,
            unitsInStock: newValue
        },
        success: function(response) {
            if (response.success) {
                showMessage('Stock updated successfully', 'success');
            } else {
                showMessage('Error updating stock: ' + response.message, 'error');
            }
        },
        error: function() {
            showMessage('Error communicating with the server', 'error');
        }
    });
}

/**
 * Displays a notification message
 */
function showMessage(message, type) {
    var alertClass = 'alert-info';
    if (type === 'success') {
        alertClass = 'alert-success';
    } else if (type === 'error') {
        alertClass = 'alert-danger';
    }
    
    var alert = $('<div class="alert ' + alertClass + '">' + message + '</div>');
    $('#messageContainer').html(alert);
    
    // Auto-hide after 5 seconds
    setTimeout(function() {
        alert.fadeOut(500, function() {
            $(this).remove();
        });
    }, 5000);
} 