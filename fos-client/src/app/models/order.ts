

    // $Classes/Enums/Interfaces(filter)[template][separator]
    // filter (optional): Matches the name or full name of the current item. * = match any, wrap in [] to match attributes or prefix with : to match interfaces or base classes.
    // template: The template to repeat for each matched item
    // separator (optional): A separator template that is placed between all templates e.g. $Properties[public $name: $Type][, ]

    // More info: http://frhagn.github.io/Typewriter/

    
     

    export class Order    {
           
        public Id: string = null;   
        public OrderDate: Date = new Date(0);   
        public IdUser: string = null;   
        public IdEvent: string = null;   
        public IdRestaurant: number = 0;   
        public IdDelivery: number = 0;   
        public IsOrdered: boolean = false;   
        public FoodDetail: { [key: number]: { [key: string]: string; }; } = {};
    }
         
