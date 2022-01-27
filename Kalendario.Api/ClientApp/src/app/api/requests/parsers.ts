import {RequestItem, RequestModel} from 'src/app/api/requests';


export function requestParser(data: any): RequestModel {
    const items: {[key: string]: RequestItem} = {};
    let itemsCount = 0;
    // for (const apt of data.appointments.map(appointmentParser)) {
    //     itemsCount += 1;
    //     if (items.hasOwnProperty(apt.employee.id)) {
    //         items[apt.employee.id].appointments.push(apt);
    //     } else {
    //         items[apt.employee.id] = {
    //             employee: apt.employee,
    //             appointments: [apt]
    //         };
    //     }
    // }
    // todo: fix here.
    return {
        ...data,
        owner: null,
        user: null,
        name: data.user?.name,
        items: Object.keys(items).map(k => items[k]),
        itemsCount
    }

}
