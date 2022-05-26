import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http'
import { ServiceResponse } from '../models/ServiceResponse';
import { AppComponent } from '../app.component';
declare let alertify: any;
@Component({
  selector: 'app-order',
  templateUrl: './order.component.html',
  styleUrls: ['./order.component.css']
})
export class OrderComponent extends AppComponent implements OnInit {
  public orders: any = [];
  public orderStatutes: any = [];
  public selectedOrder: any = {};

  public displayStyle: string = "none";
  constructor(private httpClient: HttpClient) {
    super();
  }

  ngOnInit(): void {
    this.getApiUrl();
    this.getOrders();
    this.getOrderStatutes();
  }

  getOrders() {
    var path = "order/index";
    return this.httpClient.get(this.apiUrl + path).subscribe(data => {
      this.orders = data;
    });
  }

  setSelectedOrder(order: any) {
    this.selectedOrder = order;
  }

  openPopup() {
    this.displayStyle = "block";
  }
  closePopup() {
    this.displayStyle = "none";
  }

  setOrderStatus() {
    var path = "order/UpdateOrderStatus";
    return this.httpClient.post(this.apiUrl + path, { orderId: this.selectedOrder.id, statusId: this.selectedOrder.orderStatusId }).subscribe(data => {
      var serviceResponse = data as ServiceResponse;

      if (serviceResponse.status == 1) {
        this.getOrders();
        alertify.success(serviceResponse.message);
        this.selectedOrder = {};
        this.closePopup();
      } else {
        alertify.alert(serviceResponse.message);
      }

    });
  }

  getOrderStatutes() {
    var path = "order/GetOrderStatuses";
    return this.httpClient.get(this.apiUrl + path).subscribe(data => {
      this.orderStatutes = data;
    }, err => {

    });
  }

}
