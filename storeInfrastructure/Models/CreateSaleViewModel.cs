using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace storeInfrastructure.Models;

public class CreateSaleViewModel
{
    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Please select a department")]
    public int DepartmentId { get; set; }

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Please select an employee")]
    public int EmployeeId { get; set; }

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Please select a product")]
    public int ProductId { get; set; }

    [Required]
    [Range(1, 1000, ErrorMessage = "Quantity must be between 1 and 1000")]
    public int Quantity { get; set; } = 1;

    public int? CustomerId { get; set; }

    public List<SelectListItem> Departments { get; set; } = new();
    public List<SelectListItem> Employees { get; set; } = new();
    public List<SelectListItem> Products { get; set; } = new();
    public List<SelectListItem> Customers { get; set; } = new();
}