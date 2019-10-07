

    // $Classes/Enums/Interfaces(filter)[template][separator]
    // filter (optional): Matches the name or full name of the current item. * = match any, wrap in [] to match attributes or prefix with : to match interfaces or base classes.
    // template: The template to repeat for each matched item
    // separator (optional): A separator template that is placed between all templates e.g. $Properties[public $name: $Type][, ]

    // More info: http://frhagn.github.io/Typewriter/

    
     

    export class EventAction    {
           
        public CanViewEvent: boolean = false;   
        public CanEditEvent: boolean = false;   
        public CanCloneEvent: boolean = false;   
        public CanCloseEvent: boolean = false;   
        public CanMakeOrder: boolean = false;   
        public CanSendRemind: boolean = false;   
        public CanViewOrder: boolean = false;   
        public CanViewEventSummary: boolean = false;
    }
         
