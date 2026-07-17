namespace DotRMapper.Tests.Models;

/// <summary>
/// Source model with person fields used in convention and custom mapping tests.
/// </summary>
/// <remarks>
/// Paired with <see cref="PersonDestination"/> in mapping scenarios.
/// </remarks>
public class PersonSource
{
    /// <summary>
    /// Gets or sets the person identifier.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the person's first name.
    /// </summary>
    public string FirstName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the person's last name.
    /// </summary>
    public string LastName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the person's birth date.
    /// </summary>
    /// <remarks>
    /// Used to compute age in custom mapping tests.
    /// </remarks>
    public DateTime BirthDate { get; set; }
}

/// <summary>
/// Destination model for person mapping tests with computed members.
/// </summary>
/// <remarks>
/// Includes <see cref="FullName"/> and <see cref="Age"/> populated by custom configuration.
/// </remarks>
public class PersonDestination
{
    /// <summary>
    /// Gets or sets the person identifier.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the person's first name.
    /// </summary>
    public string FirstName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the person's last name.
    /// </summary>
    public string LastName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the combined full name.
    /// </summary>
    /// <remarks>
    /// Typically mapped from first and last name via expression.
    /// </remarks>
    public string FullName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the computed age in years.
    /// </summary>
    /// <remarks>
    /// Typically derived from <see cref="PersonSource.BirthDate"/>.
    /// </remarks>
    public int Age { get; set; }
}

/// <summary>
/// Source address model used in nested object mapping tests.
/// </summary>
/// <remarks>
/// Mapped to <see cref="AddressDestination"/> by name convention.
/// </remarks>
public class AddressSource
{
    /// <summary>
    /// Gets or sets the street name.
    /// </summary>
    public string Street { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the city name.
    /// </summary>
    public string City { get; set; } = string.Empty;
}

/// <summary>
/// Source customer model with nested address and tag collection.
/// </summary>
/// <remarks>
/// Exercises nested object and collection mapping.
/// </remarks>
public class CustomerSource
{
    /// <summary>
    /// Gets or sets the customer identifier.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the customer name.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the customer's address.
    /// </summary>
    public AddressSource Address { get; set; } = new();

    /// <summary>
    /// Gets or sets the customer's tags.
    /// </summary>
    public List<string> Tags { get; set; } = [];
}

/// <summary>
/// Destination address model used in nested object mapping tests.
/// </summary>
/// <remarks>
/// Paired with <see cref="AddressSource"/>.
/// </remarks>
public class AddressDestination
{
    /// <summary>
    /// Gets or sets the street name.
    /// </summary>
    public string Street { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the city name.
    /// </summary>
    public string City { get; set; } = string.Empty;
}

/// <summary>
/// Destination customer model with nested address and tag collection.
/// </summary>
/// <remarks>
/// Paired with <see cref="CustomerSource"/>.
/// </remarks>
public class CustomerDestination
{
    /// <summary>
    /// Gets or sets the customer identifier.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the customer name.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the customer's address.
    /// </summary>
    public AddressDestination Address { get; set; } = new();

    /// <summary>
    /// Gets or sets the customer's tags.
    /// </summary>
    public List<string> Tags { get; set; } = [];
}

/// <summary>
/// Source order line item used in collection mapping tests.
/// </summary>
/// <remarks>
/// Mapped to <see cref="OrderItemDestination"/>.
/// </remarks>
public class OrderItemSource
{
    /// <summary>
    /// Gets or sets the product name.
    /// </summary>
    public string ProductName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the item price.
    /// </summary>
    public decimal Price { get; set; }
}

/// <summary>
/// Destination order line item used in collection mapping tests.
/// </summary>
/// <remarks>
/// Paired with <see cref="OrderItemSource"/>.
/// </remarks>
public class OrderItemDestination
{
    /// <summary>
    /// Gets or sets the product name.
    /// </summary>
    public string ProductName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the item price.
    /// </summary>
    public decimal Price { get; set; }
}

/// <summary>
/// Source order model with an array of line items.
/// </summary>
/// <remarks>
/// Used to test array-to-list collection mapping.
/// </remarks>
public class OrderSource
{
    /// <summary>
    /// Gets or sets the order identifier.
    /// </summary>
    public int OrderId { get; set; }

    /// <summary>
    /// Gets or sets the order line items.
    /// </summary>
    public OrderItemSource[] Items { get; set; } = [];
}

/// <summary>
/// Destination order model with a list of line items.
/// </summary>
/// <remarks>
/// Paired with <see cref="OrderSource"/>.
/// </remarks>
public class OrderDestination
{
    /// <summary>
    /// Gets or sets the order identifier.
    /// </summary>
    public int OrderId { get; set; }

    /// <summary>
    /// Gets or sets the order line items.
    /// </summary>
    public List<OrderItemDestination> Items { get; set; } = [];
}

/// <summary>
/// Source product model with an internal code field.
/// </summary>
/// <remarks>
/// Used in custom display price mapping tests.
/// </remarks>
public class ProductSource
{
    /// <summary>
    /// Gets or sets the product name.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the product price.
    /// </summary>
    public decimal Price { get; set; }

    /// <summary>
    /// Gets or sets the internal product code.
    /// </summary>
    /// <remarks>
    /// Not mapped to the destination model by default.
    /// </remarks>
    public string InternalCode { get; set; } = string.Empty;
}

/// <summary>
/// Destination product model with a formatted display price.
/// </summary>
/// <remarks>
/// Paired with <see cref="ProductSource"/>.
/// </remarks>
public class ProductDestination
{
    /// <summary>
    /// Gets or sets the product name.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the product price.
    /// </summary>
    public decimal Price { get; set; }

    /// <summary>
    /// Gets or sets the formatted display price string.
    /// </summary>
    /// <remarks>
    /// Typically populated by a custom resolver or expression.
    /// </remarks>
    public string DisplayPrice { get; set; } = string.Empty;
}

/// <summary>
/// Source status enumeration for enum mapping tests.
/// </summary>
/// <remarks>
/// Member names match <see cref="StatusDestination"/>.
/// </remarks>
public enum StatusSource
{
    /// <summary>
    /// Indicates an active status.
    /// </summary>
    /// <remarks>
    /// Maps to <see cref="StatusDestination.Active"/>.
    /// </remarks>
    Active,

    /// <summary>
    /// Indicates an inactive status.
    /// </summary>
    /// <remarks>
    /// Maps to <see cref="StatusDestination.Inactive"/>.
    /// </remarks>
    Inactive,
}

/// <summary>
/// Destination status enumeration for enum mapping tests.
/// </summary>
/// <remarks>
/// Member names match <see cref="StatusSource"/>.
/// </remarks>
public enum StatusDestination
{
    /// <summary>
    /// Indicates an active status.
    /// </summary>
    /// <remarks>
    /// Maps from <see cref="StatusSource.Active"/>.
    /// </remarks>
    Active,

    /// <summary>
    /// Indicates an inactive status.
    /// </summary>
    /// <remarks>
    /// Maps from <see cref="StatusSource.Inactive"/>.
    /// </remarks>
    Inactive,
}

/// <summary>
/// Source entity wrapping a status enum value.
/// </summary>
/// <remarks>
/// Used in enum conversion mapping tests.
/// </remarks>
public class StatusEntitySource
{
    /// <summary>
    /// Gets or sets the entity status.
    /// </summary>
    public StatusSource Status { get; set; }
}

/// <summary>
/// Destination entity wrapping a status enum value.
/// </summary>
/// <remarks>
/// Paired with <see cref="StatusEntitySource"/>.
/// </remarks>
public class StatusEntityDestination
{
    /// <summary>
    /// Gets or sets the entity status.
    /// </summary>
    public StatusDestination Status { get; set; }
}

/// <summary>
/// Source customer model with flattened address fields.
/// </summary>
/// <remarks>
/// Useful for testing custom nested mapping from flat properties.
/// </remarks>
public class FlatCustomerSource
{
    /// <summary>
    /// Gets or sets the customer identifier.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the customer name.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the address street from a flattened source shape.
    /// </summary>
    public string AddressStreet { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the address city from a flattened source shape.
    /// </summary>
    public string AddressCity { get; set; } = string.Empty;
}

/// <summary>
/// Destination customer model with a nested address object.
/// </summary>
/// <remarks>
/// Paired with <see cref="FlatCustomerSource"/> in advanced mapping scenarios.
/// </remarks>
public class NestedCustomerDestination
{
    /// <summary>
    /// Gets or sets the customer identifier.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the customer name.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the nested customer address.
    /// </summary>
    public AddressDestination Address { get; set; } = new();
}

/// <summary>
/// Minimal source model for audit callback tests.
/// </summary>
/// <remarks>
/// Mapped to <see cref="AuditDestination"/> with AfterMap enrichment.
/// </remarks>
public class AuditSource
{
    /// <summary>
    /// Gets or sets the record name.
    /// </summary>
    public string Name { get; set; } = string.Empty;
}

/// <summary>
/// Destination audit model with metadata populated after mapping.
/// </summary>
/// <remarks>
/// <see cref="CreatedBy"/> is set in AfterMap callbacks during tests.
/// </remarks>
public class AuditDestination
{
    /// <summary>
    /// Gets or sets the record name.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the creator identifier.
    /// </summary>
    /// <remarks>
    /// Not sourced from <see cref="AuditSource"/>; populated by AfterMap.
    /// </remarks>
    public string CreatedBy { get; set; } = string.Empty;
}

/// <summary>
/// Source user model for BeforeMap callback tests.
/// </summary>
/// <remarks>
/// Password may be modified before member assignment in tests.
/// </remarks>
public class UserSource
{
    /// <summary>
    /// Gets or sets the username.
    /// </summary>
    public string Username { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the password value.
    /// </summary>
    public string Password { get; set; } = string.Empty;
}

/// <summary>
/// Destination user model for BeforeMap callback tests.
/// </summary>
/// <remarks>
/// Paired with <see cref="UserSource"/>.
/// </remarks>
public class UserDestination
{
    /// <summary>
    /// Gets or sets the username.
    /// </summary>
    public string Username { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the password value.
    /// </summary>
    public string Password { get; set; } = string.Empty;
}

/// <summary>
/// Source employee model for member ignore tests.
/// </summary>
/// <remarks>
/// All properties map by convention to <see cref="EmployeeDestination"/> except ignored members.
/// </remarks>
public class EmployeeSource
{
    /// <summary>
    /// Gets or sets the employee first name.
    /// </summary>
    public string FirstName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the employee last name.
    /// </summary>
    public string LastName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the employee department.
    /// </summary>
    public string Department { get; set; } = string.Empty;
}

/// <summary>
/// Destination employee model with an unmapped badge identifier.
/// </summary>
/// <remarks>
/// <see cref="BadgeId"/> is ignored in <see cref="Profiles.EmployeeProfile"/>.
/// </remarks>
public class EmployeeDestination
{
    /// <summary>
    /// Gets or sets the employee first name.
    /// </summary>
    public string FirstName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the employee last name.
    /// </summary>
    public string LastName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the employee department.
    /// </summary>
    public string Department { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the employee badge identifier.
    /// </summary>
    /// <remarks>
    /// Left empty when ignored during mapping configuration.
    /// </remarks>
    public string BadgeId { get; set; } = string.Empty;
}
