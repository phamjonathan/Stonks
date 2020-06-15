// {Alex Trend Strategy
// Copyright January 2020, Steve Alexander
// All rights reserved
// 
// ==========================================}

inputs:
BeginBankroll(1000); //begin bankroll

variables:
midpoint(0), //midline
redline(0), //volty 50-day average
barsize(0), //bar size
volty(0), //voatility
avgvol(0), //avg volatility
tradeOK(0), //trade flag
trend("flat"), //trend
size(0), //shares to trade
Bankroll(0); //working bankroll

//==========
// Mainline
//==========

bankroll = BeginBankroll + NetProfit;
size = intportion(bankroll/close);

// Trade OK?

tradeOK = 0;
barsize = high - low;
midpoint = (high + low)/2;
volty = barsize/midpoint;
redline = average(volty,50)*100;
avgvol = average(volty,5)*100;
if avgvol < redline then tradeOK = 1;

// Find Trend

trend = "flat";
if high > high[1] and low > low[1]
then trend = "up";
if high < high[1] and low < low[1]
then trend = "down";

//==========
// Enter Trade
//==========

if marketposition = 0 and trend = "up" and tradeOK =1
then begin
    buy size shares next bar market;
    value1 = TL_New(date,time,low*.97,date,time,low*.99);
    value2 = TL_SetColor(value1,white);
    value3 = TL_SetSize(value1,4);

    //===========
    // Exit Trade
    //===========

    if marketposition > 0 and tradeOK = 0
    then begin
        sell all shares next bar market;
        value1 = TL_New(date,time,high*1.01,date,time,high*1.03);
        value2 = TL_SetColor(value1,yellow);
        value3 = TL_SetSize(value1,4);
    end;

    if marketposition > 0 and trend = "down"
    and average(close,3) < average(close,5)
    then begin
        sell all shares next bar market;
        value1 = TL_New(date,time,high*1.01,date,time,high*1.03);
        value2 = TL_SetColor(value1,yellow);
        value3 = TL_SetSize(value1,4);
    end;

    End
