

    // $Classes/Enums/Interfaces(filter)[template][separator]
    // filter (optional): Matches the name or full name of the current item. * = match any, wrap in [] to match attributes or prefix with : to match interfaces or base classes.
    // template: The template to repeat for each matched item
    // separator (optional): A separator template that is placed between all templates e.g. $Properties[public $name: $Type][, ]

    // More info: http://frhagn.github.io/Typewriter/

    
          import { Photo } from './photo';
import { Price } from './price';
import { DiscountPrice } from './discount-price';

    export class Food    {
           
        public description: string = null;   
        public id: number = 0;   
        public is_available: boolean = false;   
        public is_group_discount_item: boolean = false;   
        public photos: Photo[] = [];   
        public price: Price = null;   
        public discount_price: DiscountPrice = null;   
        public name: string = null;   
        public time: string = null;   
        public options: string = null;   
        public properties: string = null;   
        public quantity: string = null;
    }
         
