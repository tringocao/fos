

    // $Classes/Enums/Interfaces(filter)[template][separator]
    // filter (optional): Matches the name or full name of the current item. * = match any, wrap in [] to match attributes or prefix with : to match interfaces or base classes.
    // template: The template to repeat for each matched item
    // separator (optional): A separator template that is placed between all templates e.g. $Properties[public $name: $Type][, ]

    // More info: http://frhagn.github.io/Typewriter/

    
     import { Comment } from './comment';

    export class FoodReport    {
           
        public FoodId: string = null;   
        public Name: string = null;   
        public Price: number = 0;   
        public Picture: string = null;   
        public Comments: Comment[] = [];   
        public TotalComment: string = null;   
        public Amount: number = 0;   
        public Total: number = 0;   
        public NumberOfUser: number = 0;   
        public UserIds: string[] = [];
    }
         
