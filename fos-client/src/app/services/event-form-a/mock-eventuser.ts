import { EventUser } from './eventuser';

export const EventUsers: EventUser[] = [
    { name: 'Salah', email: 'Liverpool' },
    { name: 'Kane', email: 'Tottenham Hospur' },
    { name: 'Hazard', email: 'Real Madrid' },
    { name: 'Griezmann', email: 'Barcelona' },
    { name: 'Mane', email: 'Liverpool' },
];


/*
Copyright Google LLC. All Rights Reserved.
Use of this source code is governed by an MIT-style license that
can be found in the LICENSE file at http://angular.io/license
*/


// // console.log('get user photo');
// for (var i = 0; i < this.hostPickerGroup.length; i++) {
//     // var obj = this.hostPickerGroup[i].id;
//     // console.log('user photo: ' + this.hostPickerGroup[i].id + ' ' + this.hostPickerGroup[i].name)


//     this.http.get(environment.apiUrl + 'api/SPUser/GetAvatarByUserId?Id=' + this.hostPickerGroup[i].id).subscribe((data: any) => {
//         // console.log("get avatar by id");

//         // console.log(data);
//         var dataImg = "data:image/png;base64," + data.Data;
//         Object.assign(this.hostPickerGroup, {
//             firstNewAttribute: {
//                 img: dataImg
//             }
//         });
//         console.log(dataImg);
//     });

// }
// console.log('finally photo');
// for (var i = 0; i < this.hostPickerGroup.length; i++) {
//     var obj = this.hostPickerGroup[i].id;
//     console.log(this.hostPickerGroup[i].img)
// }