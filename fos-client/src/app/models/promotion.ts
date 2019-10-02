

    // $Classes/Enums/Interfaces(filter)[template][separator]
    // filter (optional): Matches the name or full name of the current item. * = match any, wrap in [] to match attributes or prefix with : to match interfaces or base classes.
    // template: The template to repeat for each matched item
    // separator (optional): A separator template that is placed between all templates e.g. $Properties[public $name: $Type][, ]

    // More info: http://frhagn.github.io/Typewriter/

    
     import { PromotionType } from './promotion-type';

    export class Promotion    {
           
        public PromotionType: PromotionType = null;   
        public Value: number = 0;   
        public IsPercent: boolean = false;   
        public Expired: Date = null;   
        public MaxDiscountAmount: number = null;   
        public MinOrderAmount: number = null;   
        public DiscountedFoodIds: { [key: number]: number; } = {};
    }
         
