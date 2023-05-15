export interface AvailableRoomsModel {
    guestNumber: number;
    dogNumber: number;
    nonStandardEquipments: Array<string>;
    arrival: Date;
    leave: Date;
}