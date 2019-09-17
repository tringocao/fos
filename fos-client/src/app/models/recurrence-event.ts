

    // $Classes/Enums/Interfaces(filter)[template][separator]
    // filter (optional): Matches the name or full name of the current item. * = match any, wrap in [] to match attributes or prefix with : to match interfaces or base classes.
    // template: The template to repeat for each matched item
    // separator (optional): A separator template that is placed between all templates e.g. $Properties[public $name: $Type][, ]

    // More info: http://frhagn.github.io/Typewriter/

    
     import { RepeateType } from './repeate-type';

    export class RecurrenceEvent    {
           
        public StartDate: Date = new Date(0);   
        public EndDate: Date = new Date(0);   
        public Title: string = null;   
        public Id: number = 0;   
        public TypeRepeat: RepeateType = null;   
        public UserId: string = null;
    }
         
