

    // $Classes/Enums/Interfaces(filter)[template][separator]
    // filter (optional): Matches the name or full name of the current item. * = match any, wrap in [] to match attributes or prefix with : to match interfaces or base classes.
    // template: The template to repeat for each matched item
    // separator (optional): A separator template that is placed between all templates e.g. $Properties[public $name: $Type][, ]

    // More info: http://frhagn.github.io/Typewriter/

    
     import { Event } from './event';
import { RestaurantExcel } from './restaurant-excel';
import { FoodReport } from './food-report';
import { UserOrder } from './user-order';
import { User } from './user';

    export class ExcelModel    {
           
        public Event: Event = null;   
        public RestaurantExcel: RestaurantExcel = null;   
        public FoodReport: FoodReport[] = [];   
        public UserOrder: UserOrder[] = [];   
        public User: User[] = [];
    }
         
