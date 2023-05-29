import { DailyReservationModel } from "./daily-reservation-model";


export interface DayOfMonthModel {
    day: Date;
    dateNumber: number;
    weekDayNumber: number;
    roomStatus: Array<DailyReservationModel>;
    
}