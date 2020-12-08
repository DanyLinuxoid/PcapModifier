# PcapModifier 
Beta 0.95

Program gives ability to read offline packet with '.pcap' extension (wireshark), intercept packets and modify packets in offline mode or on the fly (if intercepting), capable of sending packets to Web.
For now program can:
1. Accept '.pcap' files with TCP, ICMP, UDP protocols.
2. Modify every layer and every module of packet as user wants.
3. Save packet for further usage.
4. Send packet as many times as user wishes to web (simply resend it, or send after modifying).
5. Dynamically intercept packets and see it's decrypted contents 
6. Dynamically modify packet in interception process
7. Automatically modify all received packets during interception process by injecting user provided values

# Issues
Program has very strange bug, which is possibly related to legacy framework that is used =>
Packets are sended twice and are received twice as well, because of this issue, replay attack or simple "intercept-modify-resend" is not working well, as packet id's are duplicated and it breaks whole packet chain.
There was no issue found in program code. Could be environment issue...
