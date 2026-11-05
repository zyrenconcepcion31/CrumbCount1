using System;
using System.ComponentModel.DataAnnotations;

namespace CrumbCount2.Models;

public class WasteRecording
{
    public int WasteRecordingId { get; set; }

    [Required]
    [Display(Name = "Product")]
    public int ProductId { get; set; }

    [Required]
    [Range(1, 100000)]
    [Display(Name = "Quantity Wasted")]
    public int QuantityWasted { get; set; }

    [Required]
    [DataType(DataType.Date)]
    [Display(Name = "Date Recorded")]
    public DateTime DateRecorded { get; set; } = DateTime.Today;

    [Required]
    [StringLength(100)]
    [Display(Name = "Reason for Waste")]
    public string ReasonForWaste { get; set; } = "";

    [StringLength(250)]
    public string? Remarks { get; set; }
    
[Range(0, double.MaxValue)]
[Display(Name = "Unit Cost at Waste")]
public decimal UnitCostAtWaste { get; set; }


[System.ComponentModel.DataAnnotations.Schema.NotMapped]
[Display(Name = "Total Waste Cost")]
public decimal TotalWasteCost => QuantityWasted * UnitCostAtWaste;


    public Product? Product { get; set; }
}          