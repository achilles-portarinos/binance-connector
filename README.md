# Binance Asset Collector

Use this script to get a list of all your current assets in binance, both in the spot as well as in your earn wallet.

## API Keys

You need to generate an api-secret pair to access your binance account. The values can be set in the appsettings.json file.

## Binance Earn Assets

The binance API does not support fetching of the locked-staking assets out of the box. This can be done currently only manually. Login to binance in your browser, navigate to earn and locked staking,
open the browser's console tools, go to the navigation tab, select only the XHR requests, observe the "list" request and paste the contents of the fetched json file inside a new earn.json file on the root level. This is where the data gets parsed out of.