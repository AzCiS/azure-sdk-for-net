
namespace Microsoft.Azure.Management.DataBox.Models
{
    using Azure;
    using Management;
    using DataBox;
    using Rest;
    using Rest.Serialization;
    using Newtonsoft.Json;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Job Resource.
    /// </summary>
    [JsonTransformation]
    public partial class JobResource : Resource
    {
        /// <summary>
        /// Initializes a new instance of the JobResource class.
        /// </summary>
        public JobResource() { }

        /// <summary>
        /// Initializes a new instance of the JobResource class.
        /// </summary>
        /// <param name="location">The location of the resource. This will be
        /// one of the supported and registered Azure Regions (e.g. West US,
        /// East US, Southeast Asia, etc.). The region of a resource cannot be
        /// changed once it is created, but if an identical region is specified
        /// on update the request will succeed.</param>
        /// <param name="destinationAccountDetails">Destination account
        /// details.</param>
        /// <param name="details">Details of a job run. This field will only be
        /// sent for expand details filter.</param>
        /// <param name="tags">The list of key value pairs that describe the
        /// resource. These tags can be used in viewing and grouping this
        /// resource (across resource groups).</param>
        /// <param name="sku">The sku type.</param>
        /// <param name="deviceType">Type of the device to be used for the job.
        /// Possible values include: 'Pod', 'Disk', 'Cabinet'</param>
        /// <param name="isCancellable">Describes whether the job is
        /// cancellable.</param>
        /// <param name="status">Name of the stage which is in progress.
        /// Possible values include: 'DeviceOrdered', 'DevicePrepared',
        /// 'Dispatched', 'Delivered', 'PickedUp', 'AtAzureDC', 'DataCopy',
        /// 'Completed', 'CompletedWithErrors', 'Cancelled',
        /// 'Failed_IssueReportedAtCustomer', 'Failed_IssueDetectedAtAzureDC',
        /// 'Aborted'</param>
        /// <param name="startTime">Time at which the job was started in UTC
        /// ISO 8601 format.</param>
        /// <param name="error">Top level error for the job.</param>
        /// <param name="deliveryPackage">Delivery package shipping
        /// details.</param>
        /// <param name="returnPackage">Return package shipping
        /// details.</param>
        /// <param name="cancellationReason">Reason for cancellation.</param>
        /// <param name="name">Name of the object.</param>
        /// <param name="id">Id of the object.</param>
        /// <param name="type">Type of the object.</param>
        public JobResource(string location, IList<DestinationAccountDetails> destinationAccountDetails, JobDetails details, IDictionary<string, string> tags = default(IDictionary<string, string>), Sku sku = default(Sku), DeviceType? deviceType = default(DeviceType?), bool? isCancellable = default(bool?), StageName? status = default(StageName?), System.DateTime? startTime = default(System.DateTime?), Error error = default(Error), PackageShippingDetails deliveryPackage = default(PackageShippingDetails), PackageShippingDetails returnPackage = default(PackageShippingDetails), string cancellationReason = default(string), string name = default(string), string id = default(string), string type = default(string))
            : base(location, tags, sku)
        {
            DeviceType = deviceType;
            IsCancellable = isCancellable;
            Status = status;
            StartTime = startTime;
            Error = error;
            DeliveryPackage = deliveryPackage;
            ReturnPackage = returnPackage;
            DestinationAccountDetails = destinationAccountDetails;
            Details = details;
            CancellationReason = cancellationReason;
            Name = name;
            Id = id;
            Type = type;
        }

        /// <summary>
        /// Gets or sets type of the device to be used for the job. Possible
        /// values include: 'Pod', 'Disk', 'Cabinet'
        /// </summary>
        [JsonProperty(PropertyName = "properties.deviceType")]
        public DeviceType? DeviceType { get; set; }

        /// <summary>
        /// Gets or sets describes whether the job is cancellable.
        /// </summary>
        [JsonProperty(PropertyName = "properties.isCancellable")]
        public bool? IsCancellable { get; set; }

        /// <summary>
        /// Gets or sets name of the stage which is in progress. Possible
        /// values include: 'DeviceOrdered', 'DevicePrepared', 'Dispatched',
        /// 'Delivered', 'PickedUp', 'AtAzureDC', 'DataCopy', 'Completed',
        /// 'CompletedWithErrors', 'Cancelled',
        /// 'Failed_IssueReportedAtCustomer', 'Failed_IssueDetectedAtAzureDC',
        /// 'Aborted'
        /// </summary>
        [JsonProperty(PropertyName = "properties.status")]
        public StageName? Status { get; set; }

        /// <summary>
        /// Gets or sets time at which the job was started in UTC ISO 8601
        /// format.
        /// </summary>
        [JsonProperty(PropertyName = "properties.startTime")]
        public System.DateTime? StartTime { get; set; }

        /// <summary>
        /// Gets or sets top level error for the job.
        /// </summary>
        [JsonProperty(PropertyName = "properties.error")]
        public Error Error { get; set; }

        /// <summary>
        /// Gets or sets delivery package shipping details.
        /// </summary>
        [JsonProperty(PropertyName = "properties.deliveryPackage")]
        public PackageShippingDetails DeliveryPackage { get; set; }

        /// <summary>
        /// Gets or sets return package shipping details.
        /// </summary>
        [JsonProperty(PropertyName = "properties.returnPackage")]
        public PackageShippingDetails ReturnPackage { get; set; }

        /// <summary>
        /// Gets or sets destination account details.
        /// </summary>
        [JsonProperty(PropertyName = "properties.destinationAccountDetails")]
        public IList<DestinationAccountDetails> DestinationAccountDetails { get; set; }

        /// <summary>
        /// Gets or sets details of a job run. This field will only be sent for
        /// expand details filter.
        /// </summary>
        [JsonProperty(PropertyName = "properties.details")]
        public JobDetails Details { get; set; }

        /// <summary>
        /// Gets or sets reason for cancellation.
        /// </summary>
        [JsonProperty(PropertyName = "properties.cancellationReason")]
        public string CancellationReason { get; set; }

        /// <summary>
        /// Gets name of the object.
        /// </summary>
        [JsonProperty(PropertyName = "name")]
        public string Name { get; protected set; }

        /// <summary>
        /// Gets id of the object.
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public string Id { get; protected set; }

        /// <summary>
        /// Gets type of the object.
        /// </summary>
        [JsonProperty(PropertyName = "type")]
        public string Type { get; protected set; }

        /// <summary>
        /// Validate the object.
        /// </summary>
        /// <exception cref="ValidationException">
        /// Thrown if validation fails
        /// </exception>
        public override void Validate()
        {
            base.Validate();
            if (DestinationAccountDetails == null)
            {
                throw new ValidationException(ValidationRules.CannotBeNull, "DestinationAccountDetails");
            }
            if (Details == null)
            {
                throw new ValidationException(ValidationRules.CannotBeNull, "Details");
            }
            if (Error != null)
            {
                Error.Validate();
            }
            if (DestinationAccountDetails != null)
            {
                foreach (var element in DestinationAccountDetails)
                {
                    if (element != null)
                    {
                        element.Validate();
                    }
                }
            }
            if (Details != null)
            {
                Details.Validate();
            }
        }
    }
}

