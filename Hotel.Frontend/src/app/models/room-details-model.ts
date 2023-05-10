import { ReservationDetailsModel } from "./reservation-details-model";

export interface RoomDetailsModel {
  id: number,
  name: string,
  price: number,
  numberOfBeds: number,
  description: string,
  size: string,
  longDescription: string,
  available: boolean,
  equipmentNames: Array<string>,
  imageURLs: Array<string>,
  reservations: Array<ReservationDetailsModel>
}
