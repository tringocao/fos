export default interface Event {
  restaurant: string;
  category: string;
  closeTime: Date;
  participants: string;
  maximumBudget: number;
  hostName: string;
  hostId: string;
  name: string;
  createdBy: string;
  eventId: string;
  status: string;
  remindTime: Date;
  isMyEvent: boolean;
}

export interface EventModel {
  Restaurant: string;
  Category: string;
  CloseTime: Date;
  Participants: string;
  MaximumBudget: number;
  HostName: string;
  HostId: string;
  Name: string;
  CreatedBy: string;
  EventId: string;
  Status: string;
  RemindTime: Date;
  IsMyEvent: boolean;
}

export interface EventResponse {
  Data: EventModel[];
  Success: boolean;
  ErrorMessage: string;
}
