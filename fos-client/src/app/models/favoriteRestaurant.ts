class FavoriteRestaurant {
    UserId: string;
    RestaurantId: string;

    constructor(userId:string, restaurantId:string) {
        this.UserId = userId;
        this.RestaurantId = restaurantId;
    }
}