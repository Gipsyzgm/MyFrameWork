//
//  PaymentViewController.m
//  IAPPayTest
//
//  Created by silicon on 14-10-28.
//  Copyright (c) 2014ƒÍ silicon. All rights reserved.
//

#import "PaymentViewController.h"
#import <UIKit/UIKit.h>


@interface PaymentViewController ()
@property(nonatomic,strong)NSString *buyName;


@property(nonatomic,strong)SKProductsRequest *request;
@end


static PaymentViewController *payVC = nil;
@implementation PaymentViewController





#if defined(__cplusplus)
extern "C"{ 
#endif  

//Unity外部接口,调用下面的外部方法前先调用这个
void initByUnity()
{
    NSLog(@"-------------initByUnity----------------");
    if (payVC == nil)
    {
        payVC = [PaymentViewController GetSingletenVC];
    }
}
//Unity请求商品
void requestProductData(char *p)
{
    NSLog(@"-------------requestProductData----------------");
    
    //NSString *productss = @"com.mjoy.sheji3d1.buy1000";
    if([SKPaymentQueue canMakePayments]){
        NSLog(@"purchaseFunc ok");
        
        NSString * list = [NSString stringWithUTF8String:p];
        
        //[PaymentViewController requestProductData1:list];
        PaymentViewController *vc = [PaymentViewController GetSingletenVC];
        [vc requestProductData1:list];
        //[self requestProductData1(list)];                     
    }else{
        NSLog(@"bu neng nei gou");
    }   
}
//Unity取消购买
void cancelProductData()
{
    NSLog(@"-------------cancelProductData----------------");
     if([SKPaymentQueue canMakePayments]){
         PaymentViewController *vc = [PaymentViewController GetSingletenVC];
         [vc cancelRequeat];
     }
}
//Unity请求恢复
void restoreByUnity()
{
    NSLog(@"-------------restoreByUnity----------------");
    if (payVC != nil)
    {
        NSLog(@"payVC=====restoreProductData");
        [payVC restoreProductData];
    }
}
    
#if defined(__cplusplus)    
}
#endif





+(PaymentViewController*)GetSingletenVC
{
    if(payVC == nil)
    {
        payVC = [[self alloc]init];
        [[SKPaymentQueue defaultQueue] addTransactionObserver:payVC];
    }
    return payVC;
}



//请求恢复商品
- (void)restoreProductData
{
    NSLog(@"-------------restoreProductData----------------");    
     [[SKPaymentQueue defaultQueue] restoreCompletedTransactions];
}

//请求商品
- (void)requestProductData1:(NSString *)type{
    
    //type = @"com.mjoy.sheji3d1.buy500";
    
    self.buyName = type;
    
    NSLog(@"-------------requestProductData1:%@----------------",type);
    NSArray *product = [[NSArray alloc] initWithObjects:type, nil];
    
    NSSet *nsset = [NSSet setWithArray:product];
    self.request = [[SKProductsRequest alloc] initWithProductIdentifiers:nsset];
    self.request.delegate = self;
    [self.request start];
    
}



//收到产品返回信息
- (void)productsRequest:(SKProductsRequest *)request didReceiveResponse:(SKProductsResponse *)response{
    NSLog(@"======%@",response);
    NSLog(@"---------------productsRequest--------------------");
    NSArray *product = response.products;
    if([product count] == 0){
        NSLog(@"--------------product count] == 0------------------");
        return;
    }
    
    NSLog(@"productID:%@", response.invalidProductIdentifiers);
    NSLog(@"product count:%d",[product count]);
    
    SKProduct *p = nil;
    for (SKProduct *pro in product) {
        NSLog(@"%@", [pro description]);
        NSLog(@"%@", [pro localizedTitle]);
        NSLog(@"%@", [pro localizedDescription]);
        NSLog(@"%@", [pro price]);
        NSLog(@"%@", [pro productIdentifier]);
        
        p = pro;
    }
    
    SKPayment *payment = [SKPayment paymentWithProduct:p];
    
    [[SKPaymentQueue defaultQueue] addPayment:payment];
}

//«Î«Û ß∞‹
- (void)request:(SKRequest *)request didFailWithError:(NSError *)error{
    NSLog(@"------------------request err-----------------:%@", error);
}

- (void)requestDidFinish:(SKRequest *)request{
    NSLog(@"------------requestDidFinish-----------------");
}


//监听购买结果
- (void)paymentQueue:(SKPaymentQueue *)queue updatedTransactions:(NSArray *)transaction{
    for(SKPaymentTransaction *tran in transaction){
        
        switch (tran.transactionState) {
            case SKPaymentTransactionStatePurchased:
                //if (self.buyName != nil && (![self.buyName isEqualToString:@""]))
                //{
                    NSLog(@"交易完成");
                    UnitySendMessage("IAP", "BuySuccess" ,[tran.payment.productIdentifier UTF8String] );
                    [self completeTransaction:tran];
                //}
                break;
            case SKPaymentTransactionStatePurchasing:
                NSLog(@"商品添加进列表");
                
                break;
            case SKPaymentTransactionStateRestored:
                
                if (tran != nil && tran.payment != nil) {
                    NSLog(@"已经购买过商品:%@", tran.payment.productIdentifier);
                    
                    UnitySendMessage("IAP", "BuyAlready" ,[tran.payment.productIdentifier UTF8String] );
                    
                    
                    [[SKPaymentQueue defaultQueue] finishTransaction:tran];
                }
                
                
                //[self completeRestore:tran];
                
                break;
            case SKPaymentTransactionStateFailed:
                if (self.buyName != nil && (![self.buyName isEqualToString:@""]))
                {
                    NSLog(@"交易失败");
                
				    UnitySendMessage("IAP", "BuyFailed" ,[tran.payment.productIdentifier UTF8String] );

                    [self cancelTransaction];
                }
				
                break;
            default:
                break;
        }
    }
}

//恢复结束
- (void)completeRestore:(SKPaymentTransaction *)transaction{
    
    NSLog(@"恢复结束");
    //交易验证
    NSURL *recepitURL = [[NSBundle mainBundle] appStoreReceiptURL];
    NSData *receipt = [NSData dataWithContentsOfURL:recepitURL];
    
    if(!receipt){
        
    }
    
    NSError *error;
    //NSDictionary *requestContents = @{
    //                                  @"receipt-data": [receipt base64EncodedStringWithOptions:0]
    //                                  };
    //NSData *requestData = [NSJSONSerialization dataWithJSONObject:requestContents
    //                                                      options:0
    //                                                        error:&error];
    
    //if (!requestData) { /* ... Handle error ... */ }
    
    //In the test environment, use https://sandbox.itunes.apple.com/verifyReceipt
    //In the real environment, use https://buy.itunes.apple.com/verifyReceipt
    // Create a POST request with the receipt data.
    NSURL *storeURL = [NSURL URLWithString:@"https://buy.itunes.apple.com/verifyReceipt"];
    NSMutableURLRequest *storeRequest = [NSMutableURLRequest requestWithURL:storeURL];
    [storeRequest setHTTPMethod:@"POST"];
    //[storeRequest setHTTPBody:requestData];
    
    // Make a connection to the iTunes Store on a background queue.
    NSOperationQueue *queue = [[NSOperationQueue alloc] init];
    [NSURLConnection sendAsynchronousRequest:storeRequest queue:queue
                           completionHandler:^(NSURLResponse *response, NSData *data, NSError *connectionError) {
                               if (connectionError) {
                                   /* ... Handle error ... */
                                   NSLog(@"恢复验证出错");

								   UnitySendMessage("IAP", "RestoreFailed" ,[transaction.payment.productIdentifier UTF8String] );

                               } else {
                                   NSError *error;
                                   NSDictionary *jsonResponse = [NSJSONSerialization JSONObjectWithData:data options:0 error:&error];
                                   
                                   if (!jsonResponse) { /* ... Handle error ...*/ }
                                   /* ... Send a response back to the device ... */
                                   //Parse the Response
                                   NSLog(@"恢复验证成功");
                                  
                                   
                                   UnitySendMessage("IAP", "RestoreSuccess" ,[transaction.payment.productIdentifier UTF8String] );
                               }
                           }];
    
    
    
    //UnitySendMessage("IAP", "Buy" ,[transaction.payment.productIdentifier UTF8String] );
                                   
    [[SKPaymentQueue defaultQueue] finishTransaction:transaction];
    
    
}

//交易结束
- (void)completeTransaction:(SKPaymentTransaction *)transaction{

    NSLog(@"交易结束");  
    //交易验证  
    NSURL *recepitURL = [[NSBundle mainBundle] appStoreReceiptURL];  
    NSData *receipt = [NSData dataWithContentsOfURL:recepitURL];  
      
    if(!receipt){  
          
    }  
      
    NSError *error;  
    NSDictionary *requestContents = @{  
                                      @"receipt-data": [receipt base64EncodedStringWithOptions:0]  
                                      };  
    NSData *requestData = [NSJSONSerialization dataWithJSONObject:requestContents  
                                                          options:0  
                                                            error:&error];  
      
    if (!requestData) { /* ... Handle error ... */ }  
      
    //In the test environment, use https://sandbox.itunes.apple.com/verifyReceipt  
    //In the real environment, use https://buy.itunes.apple.com/verifyReceipt  
    // Create a POST request with the receipt data.  
    NSURL *storeURL = [NSURL URLWithString:@"https://buy.itunes.apple.com/verifyReceipt"];  
    NSMutableURLRequest *storeRequest = [NSMutableURLRequest requestWithURL:storeURL];  
    [storeRequest setHTTPMethod:@"POST"];  
    [storeRequest setHTTPBody:requestData];  
      
    // Make a connection to the iTunes Store on a background queue.  
    NSOperationQueue *queue = [[NSOperationQueue alloc] init];  
    [NSURLConnection sendAsynchronousRequest:storeRequest queue:queue  
                           completionHandler:^(NSURLResponse *response, NSData *data, NSError *connectionError) {  
                               if (connectionError) {  
                                   /* ... Handle error ... */  
                               } else {  
                                   NSError *error;  
                                   NSDictionary *jsonResponse = [NSJSONSerialization JSONObjectWithData:data options:0 error:&error];
                                   
                                   if (!jsonResponse) { /* ... Handle error ...*/ }  
                                   /* ... Send a response back to the device ... */  
                                   //Parse the Response  
                               }  
                           }];  
  
      
      
    [[SKPaymentQueue defaultQueue] finishTransaction:transaction]; 
    
    //UnitySendMessage("IAP", "Buy" ,[self.buyName UTF8String] );
    //UnitySendMessage("IAP", "Buy" ,[transaction.payment.productIdentifier UTF8String] );
    
}

//交易取消
- (void)cancelTransaction{

	NSLog(@"cancelTransaction");  

	UnitySendMessage("IAP", "BuyCancle", "");
}
- (void)applicationWillDidEnterBackground:(NSNotification *)notification {
    
    NSLog(@"background");
   [self.request cancel];
    
}

- (void)cancelRequeat {
    
    NSLog(@"cancelRequeat");
    [self.request cancel];
    
    
}



- (void)dealloc{
    [[SKPaymentQueue defaultQueue] removeTransactionObserver:self];
    [[NSNotificationCenter defaultCenter] removeObserver:self];
    //[super dealloc];
}

@end
