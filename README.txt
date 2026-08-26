KUMAR STUDIO - 100% FIXED BOOKING + GALLERY CROSS DEVICE
Admin Password: kumar@123
Logo: New Fujifilm X-H2S design

BUG 1 FIXED - Booking not showing in admin:
- Same localStorage key kumarBookings for booking and admin
- Booking submit saves and shows instantly in admin, Refresh button added
- Test: Make booking then go Admin > Bookings > Refresh

BUG 2 FIXED - Photos not showing on other mobile:
- Upload method SAME: Photo direct base64, Video small <8MB direct + YouTube link
- Cross-device sync: Admin Gallery Manager has Export Gallery Link and Import Gallery for manual sync between devices (because static site localStorage is per-device)
- Gallery key kumarGallery same everywhere
- YouTube link option restored in Videos/Reels/Highlights

OTHER:
- Logo change compress 600px quality 0.8 preview no white screen
- Full Edit Access: Bookings View/Edit/Delete/Status, Bills Edit, Gallery Edit/Replace/UpDown/ExportImport
- Bills with letterhead UPI QR

DEPLOY: netlify.com > Deploy manually > index.html drag drop
