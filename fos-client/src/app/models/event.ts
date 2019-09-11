

    // $Classes/Enums/Interfaces(filter)[template][separator]
    // filter (optional): Matches the name or full name of the current item. * = match any, wrap in [] to match attributes or prefix with : to match interfaces or base classes.
    // template: The template to repeat for each matched item
    // separator (optional): A separator template that is placed between all templates e.g. $Properties[public $name: $Type][, ]

    // More info: http://frhagn.github.io/Typewriter/

    
    import { EventAction } from './event-action';

    export class Event    {
           
        public Name: string = null;   
        public Restaurant: string = null;   
        public RestaurantId: string = null;   
        public Category: string = null;   
        public Participants: string = null;   
        public MaximumBudget: string = null;   
        public DeliveryId: string = null;   
        public HostName: string = null;   
        public HostId: string = null;   
        public CreatedBy: string = null;   
        public CloseTime: Date = null;   
        public EventId: string = null;   
        public Status: string = null;   
        public RemindTime: Date = null;   
        public IsMyEvent: boolean = false;   
        public EventType: string = null;   
        public EventDate: Date = null;   
        public EventParticipantsJson: string = null;   
        public Action: EventAction = null;
    }
         
