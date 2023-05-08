export interface RoomListModel {
    id: number;
    name: string;
    price: number;
    numberOfBeds: number;
    description: string;
    available: true;
    equipmentNames: Array<string>;
    imageURLs: Array<string>;
}